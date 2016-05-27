using System;
using System.Reflection;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets;
using PlanetarySystem.AstronomicalObjects.Stars;
using PlanetarySystem.AstronomicalObjects;
using System.IO;
using PlanetarySystem.AdapterPattern;
using PlanetarySystem.CommandPattern;
namespace PlanetarySystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlanetarySystemObjects ObjectsList = new PlanetarySystemObjects();
        AstronomicalObject ObjectInFocus;
        Star SytemCenter;
        private const int Scale = 15;
        List <PluginInterface.IPlugin> PluginList;
        SerializationManager serializationManager;
        PluginManager PluginManager;
        public MainWindow()
        {
            InitializeComponent();
            PluginList = LoadPlugins(Directory.GetCurrentDirectory() + "\\plugins");
            PluginManager = new PluginManager(PluginList);
            serializationManager = new SerializationManager();
        }

        public void LoadSavedData(string filename)
        {
            ISerializer serializer = null;
            switch (filename)
            {
                case "BinarySerializer.plansys":
                    serializer = new BinarySerializer();
                    break;
                case "XmlSerializer.plansys":
                    serializer = new XmlSerializer();
                    break;
                case "TxtSerializer.plansys":
                    serializer = new TxtSerializer();
                    break;
                default:
                    serializer = new JsonSerializer(filename, PluginManager);
                    break;
            }
            if (serializer != null)
            {
                serializationManager.deserializeCommand = new DeserializeCommand(serializer, ObjectsList.ObjectsList);
                ObjectsList.ObjectsList = serializationManager.LaunchDeserialization(filename);

                initializeFirstComponent();
            }
        }
   
        private void SaveButtonHandler(object sender, RoutedEventArgs e)
        {
            ISerializer serializer = null;
            string filename;
            string ext = ".plansys";
            switch (comboBoxSerialization.Text)
            {
                case "Бинарная":
                    serializer = new BinarySerializer();
                    break;
                case "XML":
                    serializer = new XmlSerializer();
                    break;
                case "Текстовая":
                    serializer = new TxtSerializer();
                    break;
                case "Сохранить в JSON":
                    serializer = new JsonSerializer(comboBoxPlugins.Text, PluginManager);
                    break;
            }

            filename = InterfaceBuilder.TrimClassName(serializer.GetType().ToString()) + ext;

            serializationManager.serializeCommand = new SerializeCommand(serializer, ObjectsList.ObjectsList);
            serializationManager.LaunchSerialization(filename);
        }

        private List<PluginInterface.IPlugin> LoadPlugins(string dir)
        {
            List<PluginInterface.IPlugin> PluginList = new List<PluginInterface.IPlugin>();
            PluginInterface.IPlugin plugin = null;
            try
            {
                foreach (var file in Directory.EnumerateFiles(dir, "*.dll", SearchOption.AllDirectories))
                {
                    if (file.Contains("Decorator.dll"))
                        continue;            
                    Assembly asm = Assembly.LoadFrom(file);

                    foreach (Type t in asm.GetExportedTypes())
                    {
                        if (typeof(PluginInterface.IPlugin).IsAssignableFrom(t))
                        {                            
                            plugin = (PluginInterface.IPlugin)asm.CreateInstance(t.FullName);
                            PluginList.Add(plugin);
                            comboBoxPlugins.Items.Add(plugin.GetType().Name);                                                      
                        }
                    }
                }
                return PluginList;
            }
            catch
            {
                MessageBox.Show(string.Format("Проблема при сканировании директории с плагинами."), "Ошибка при загрузке плагинов");
                return null;
            }
        }
       

        private void OpenSaveDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();

            dialog.Filter = "Saved planetary systems (.plansys)|*.plansys";
            foreach (PluginInterface.IPlugin p in PluginList)
            {
               string pluginFilter =  "|" + p.GetType().Name + " saves |*." + p.extension;
                dialog.Filter += pluginFilter;
            }
           
            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.SafeFileName;
               
                    LoadSavedData(filename);
            }
        }

        void BuildSystemModel(PlanetarySystemObjects ObjectsList, Canvas DrawingField)
        {
            DrawingField.Children.Clear();
            foreach (AstronomicalObject CelestialBody in ObjectsList.ObjectsList)
            {
                Draw(CelestialBody);
            }

        }
        void Draw(AstronomicalObject StarObject)
        {

            int Distance;
            int Radius;
            Brush Color;
            InterfaceBuilder.getGraphicalObjectProperties(StarObject, out Distance, out Color, out Radius);

            Ellipse CelestialBody = new Ellipse();
            CelestialBody.Name = StarObject.Name;
            CelestialBody.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e) { ObjectInFocus = StarObject;  LoadCelestialBodyProperties(AddPanel(CelestialBody.Name)); };

            CelestialBody.Width = Radius * Scale;
            CelestialBody.Height = Radius * Scale;

            int Y = (int)(DrawingField.ActualHeight / 2 - (Distance) * Scale);
            int X = (int)(DrawingField.ActualWidth / 2 - (Distance+ Radius) *Scale );

            Canvas.SetTop(CelestialBody, Y);
            Canvas.SetLeft(CelestialBody, X);

            CelestialBody.Fill = Color;

            DrawingField.Children.Add(CelestialBody);
        }


        private string AddPanel(string Name)
        {
            string PanelType = "Default";
            foreach (AstronomicalObject CelestialBody in ObjectsList.ObjectsList)
            {
                if (CelestialBody.Name.Equals(Name))
                {
                    PanelType = CelestialBody.GetType().ToString();
                    break;
                }

            }
            PanelType = InterfaceBuilder.TrimClassName(PanelType);
            StackPanel Panel = (StackPanel)App.Current.MainWindow.FindName(PanelType+"Panel");

            makeOtherControlsInvisible();
            Panel.Visibility = Visibility.Visible;
            makeChildrenVisible(Panel);

            return PanelType;
        }
        private void initializeFirstComponent()
        {
            SytemCenter = (Star)ObjectsList.ObjectsList[0];
            ObjectInFocus = ObjectsList.ObjectsList[0];
            LoadCelestialBodyProperties("Star");
            BuildSystemModel(ObjectsList, DrawingField);
        }

        private void makeChildrenVisible(StackPanel Panel)
        {
            foreach (object ChildObject in Panel.Children)
            {
                if (ChildObject.GetType() == typeof(StackPanel))
                {
                    StackPanel Child = (StackPanel)ChildObject;
                    Child.Visibility = Visibility.Visible;
                }

            }
        }

        private void makeOtherControlsInvisible()
        {
            ChildControls ccChildren = new ChildControls();
            foreach (object o in ccChildren.GetChildren(App.Current.MainWindow, 1))
                {
                    if (o.GetType() == typeof(StackPanel))
                    {
                        StackPanel pnl = (StackPanel)o;
                         pnl.Visibility = Visibility.Hidden;
                    }
                }
        }

        private void UpdateUI(object sender, SizeChangedEventArgs e)
        {
            BuildSystemModel(ObjectsList, DrawingField);

        }

        private void UpdateUI(object sender, RoutedEventArgs e)
        {
            BuildSystemModel(ObjectsList, DrawingField);
        }

        private void textBox_Copy2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LoadCelestialBodyProperties(string PanelType)
        {
           ShowProperties(PanelType);
        }

        private void StarPanel_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ShowProperties(string sender)
        {
            switch (sender)
            {
                case "Star":
                        Star CurrentObject = (Star)ObjectInFocus;
                        textBoxName.Text = CurrentObject.Name;
                        textBoxMass.Text = CurrentObject.Mass.ToString();
                        textBoxRadius.Text = CurrentObject.Radius.ToString();
                        textBoxSpectralClass.Text = CurrentObject.StarColor;
                        textBoxTemperature.Text = CurrentObject.Temperature.ToString();
                        textBoxRotationPeriod.Text = CurrentObject.RotationPeriod.ToString();
                        textBoxType.Text = CurrentObject.StarType;
                    break;
                case "GasGiant":
                    GasGiant CurrentGasGiant = (GasGiant)ObjectInFocus;
                    textBoxNameGG.Text = CurrentGasGiant.Name;
                    textBoxMassGG.Text = CurrentGasGiant.Mass.ToString();
                    textBoxRadiusGG.Text = CurrentGasGiant.Radius.ToString();
                    textBoxTemperatureGG.Text = CurrentGasGiant.Temperature.ToString();
                    textBoxRotationPeriodGG.Text = CurrentGasGiant.RotationPeriod.ToString();
                    textBoxOrbitalPeriodGG.Text = CurrentGasGiant.OrbitalPeriod.ToString();
                    textBoxPrevailngElementGG.Text = CurrentGasGiant.PrevailngElement;
                    textBoxAtmosphereDepthGG.Text = CurrentGasGiant.AtmosphereDepth.ToString();
                    textBoxDistanceGG.Text = CurrentGasGiant.DistanceToTheStar.ToString();
                    textBoxCentralBodyGG.Text = CurrentGasGiant.CentralBody.Name;
                    textBoxSatellitesGG.Text = CurrentGasGiant.GetSatellites();
                    break;

                case "TerrestrialPlanet":
                    TerrestrialPlanet CurrentTerrestrialPlanet = (TerrestrialPlanet)ObjectInFocus;
                    textBoxNameTP.Text = CurrentTerrestrialPlanet.Name;
                    textBoxCompositionTP.Text = CurrentTerrestrialPlanet.Сomposition;
                    textBoxFluidWaterTP.Text = CurrentTerrestrialPlanet.FluidWater.ToString();
                    textBoxLifeTP.Text = CurrentTerrestrialPlanet.Life.ToString();
                    textBoxMassTP.Text = CurrentTerrestrialPlanet.Mass.ToString();
                    textBoxRadiusTP.Text = CurrentTerrestrialPlanet.Radius.ToString();
                    textBoxTemperatureTP.Text = CurrentTerrestrialPlanet.Temperature.ToString();
                    textBoxRotationPeriodTP.Text = CurrentTerrestrialPlanet.RotationPeriod.ToString();
                    textBoxOrbitalPeriodTP.Text = CurrentTerrestrialPlanet.OrbitalPeriod.ToString();
                    textBoxPrevailngElementTP.Text = CurrentTerrestrialPlanet.PrevailngElement;
                    textBoxAtmosphereDepthTP.Text = CurrentTerrestrialPlanet.AtmosphereDepth.ToString();
                    textBoxDistanceTP.Text = CurrentTerrestrialPlanet.DistanceToTheStar.ToString();
                    textBoxCentralBodyTP.Text = CurrentTerrestrialPlanet.CentralBody.Name;
                    textBoxSatellitesTP.Text = CurrentTerrestrialPlanet.GetSatellites();
                    break;
                case "Comet":
                    Comet CurrentComet = (Comet)ObjectInFocus;
                    textBoxNameComet.Text = CurrentComet.Name;
                    textBoxLifeComet.Text = CurrentComet.Life.ToString();
                    textBoxMassComet.Text = CurrentComet.Mass.ToString();
                    textBoxRadiusComet.Text = CurrentComet.Radius.ToString();
                    textBoxOrbitalPeriodComet.Text = CurrentComet.OrbitalPeriod.ToString();
                    textBoxDistanceComet.Text = CurrentComet.DistanceToTheStar.ToString();
                    textBoxTailLengthComet.Text = CurrentComet.TailLength.ToString();
                    textBoxOrbitComet.Text = CurrentComet.OrbitSize.ToString();
                    break;
                case "Asteroid":
                    Asteroid CurrentAsteroid = (Asteroid)ObjectInFocus;
                    textBoxNameAsteroid.Text = CurrentAsteroid.Name;
                    textBoxLifeAsteroid.Text = CurrentAsteroid.Life.ToString();
                    textBoxMassAsteroid.Text = CurrentAsteroid.Mass.ToString();
                    textBoxRadiusAsteroid.Text = CurrentAsteroid.Radius.ToString();
                    textBoxOrbitalPeriodAsteroid.Text = CurrentAsteroid.OrbitalPeriod.ToString();
                    textBoxDistanceAsteroid.Text = CurrentAsteroid.DistanceToTheStar.ToString();
                    textBoxCompositionAsteroid.Text = CurrentAsteroid.Composition;
                    textBoxShapeAsteroid.Text = CurrentAsteroid.Shape;
                    break;
            }

        }
        private void ChangeShapeName(string Name, string NewName)
        {
            foreach (object ChildObject in DrawingField.Children)
            {
                if (ChildObject.GetType() == typeof(Ellipse))
                {

                    Ellipse Child = (Ellipse)ChildObject;
                    if (Child.Name == Name)
                        Child.Name = NewName;
                }

            }
        }
        private void StarPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Star CurrentObject = (Star)ObjectInFocus;
                ChangeShapeName(CurrentObject.Name, textBoxName.Text);


                CurrentObject.Name = textBoxName.Text;
                CurrentObject.SetType(Double.Parse(textBoxRadius.Text));
                CurrentObject.SetSpectralClass(Double.Parse(textBoxTemperature.Text));
                CurrentObject.Mass = Double.Parse(textBoxMass.Text);
                CurrentObject.RotationPeriod = Double.Parse(textBoxRotationPeriod.Text);

                ShowProperties(InterfaceBuilder.TrimClassName(CurrentObject.GetType().ToString()));
                BuildSystemModel(ObjectsList, DrawingField);
            }
        }

        private void GasGiantPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GasGiant CurrentGasGiant = (GasGiant)ObjectInFocus;
                ChangeShapeName(CurrentGasGiant.Name, textBoxNameGG.Text);

                CurrentGasGiant.Name = textBoxNameGG.Text;
                CurrentGasGiant.Mass = Double.Parse(textBoxMassGG.Text);
                CurrentGasGiant.Radius = Double.Parse(textBoxRadiusGG.Text);
                CurrentGasGiant.Temperature = Double.Parse(textBoxTemperatureGG.Text);
                CurrentGasGiant.RotationPeriod = Double.Parse(textBoxRotationPeriodGG.Text);
                CurrentGasGiant.OrbitalPeriod = Double.Parse(textBoxOrbitalPeriodGG.Text);
                CurrentGasGiant.PrevailngElement = textBoxPrevailngElementGG.Text;
                CurrentGasGiant.AtmosphereDepth = Double.Parse(textBoxAtmosphereDepthGG.Text);
                CurrentGasGiant.DistanceToTheStar = Double.Parse(textBoxDistanceGG.Text);
              

                ShowProperties(InterfaceBuilder.TrimClassName(CurrentGasGiant.GetType().ToString()));
                BuildSystemModel(ObjectsList, DrawingField);
            }
        }

        private void TerrestrialPlanetPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TerrestrialPlanet CurrentTerrestrialPlanet = (TerrestrialPlanet)ObjectInFocus;
                ChangeShapeName(CurrentTerrestrialPlanet.Name, textBoxNameTP.Text);

                CurrentTerrestrialPlanet.Name = textBoxNameTP.Text;
                CurrentTerrestrialPlanet.Mass = Double.Parse(textBoxMassTP.Text);
                CurrentTerrestrialPlanet.Radius = Double.Parse(textBoxRadiusTP.Text);
                CurrentTerrestrialPlanet.Temperature = Double.Parse(textBoxTemperatureTP.Text);
                CurrentTerrestrialPlanet.RotationPeriod = Double.Parse(textBoxRotationPeriodTP.Text);
                CurrentTerrestrialPlanet.OrbitalPeriod = Double.Parse(textBoxOrbitalPeriodTP.Text);
                CurrentTerrestrialPlanet.PrevailngElement = textBoxPrevailngElementTP.Text;
                CurrentTerrestrialPlanet.AtmosphereDepth = Double.Parse(textBoxAtmosphereDepthTP.Text);
                CurrentTerrestrialPlanet.DistanceToTheStar = Double.Parse(textBoxDistanceTP.Text);
                CurrentTerrestrialPlanet.Сomposition = textBoxCompositionTP.Text;
                CurrentTerrestrialPlanet.FluidWater = Boolean.Parse(textBoxFluidWaterTP.Text);
                CurrentTerrestrialPlanet.Life = Boolean.Parse(textBoxLifeTP.Text);                

                ShowProperties(InterfaceBuilder.TrimClassName(CurrentTerrestrialPlanet.GetType().ToString()));
                BuildSystemModel(ObjectsList, DrawingField);
            }
        }

        private void CometPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Comet CurrentComet = (Comet)ObjectInFocus;

                ChangeShapeName(CurrentComet.Name, textBoxNameComet.Text);

                CurrentComet.Name = textBoxNameComet.Text;
                CurrentComet.Life = Boolean.Parse(textBoxLifeComet.Text);
                CurrentComet.Mass = Double.Parse(textBoxMassComet.Text);
                CurrentComet.Radius = Double.Parse(textBoxRadiusComet.Text);
                CurrentComet.OrbitalPeriod = Double.Parse(textBoxOrbitalPeriodComet.Text);
                CurrentComet.DistanceToTheStar = Double.Parse(textBoxDistanceComet.Text);
                CurrentComet.TailLength = Double.Parse(textBoxTailLengthComet.Text);
                CurrentComet.OrbitSize = Double.Parse(textBoxOrbitComet.Text);

                ShowProperties(InterfaceBuilder.TrimClassName(CurrentComet.GetType().ToString()));
                BuildSystemModel(ObjectsList, DrawingField);
            }
        }

        private void AsteroidPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Asteroid CurrentAsteroid = (Asteroid)ObjectInFocus;

                ChangeShapeName(CurrentAsteroid.Name, textBoxNameAsteroid.Text);

                CurrentAsteroid.Name = textBoxNameAsteroid.Text;
                CurrentAsteroid.Life = Boolean.Parse(textBoxLifeAsteroid.Text);
                CurrentAsteroid.Mass = Double.Parse(textBoxMassAsteroid.Text);
                CurrentAsteroid.Radius = Double.Parse(textBoxRadiusAsteroid.Text);
                CurrentAsteroid.OrbitalPeriod = Double.Parse(textBoxOrbitalPeriodAsteroid.Text);
                CurrentAsteroid.DistanceToTheStar = Double.Parse(textBoxDistanceAsteroid.Text);
                CurrentAsteroid.Composition = textBoxCompositionAsteroid.Text;
                CurrentAsteroid.Shape = textBoxShapeAsteroid.Text;

                ShowProperties(InterfaceBuilder.TrimClassName(CurrentAsteroid.GetType().ToString()));
                BuildSystemModel(ObjectsList, DrawingField);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            string NewObjectName = "";

            if (tbNewObjName.Text.Trim().Equals(""))
                NewObjectName = "Alpha" + (new Random()).Next(99, 9999).ToString();
            else
                NewObjectName = tbNewObjName.Text.Trim();
            foreach (AstronomicalObject obj in ObjectsList.ObjectsList)
            {
                if (obj.Name.Equals(NewObjectName))
                    NewObjectName += (new Random()).Next(99, 9999).ToString();
            }

            switch (comboBoxTypes.Text)
            {
                case "Звезда":
                    bool StarAlreadyExsists = false;
                    foreach (AstronomicalObject obj in ObjectsList.ObjectsList)
                    {
                        if (obj.GetType().Name == "Star")                            
                            StarAlreadyExsists = true;
                    }
                    if (StarAlreadyExsists == true)
                        MessageBox.Show("В системе уже есть звезда.");
                    else
                    {                       
                        SytemCenter = new Star(NewObjectName);
                        ObjectsList.ObjectsList.Add(SytemCenter);                        
                    }                        
                    break;
                case "Твердотельная планета":
                    ObjectsList.ObjectsList.Add(new TerrestrialPlanet(NewObjectName, SytemCenter, ObjectsList.ObjectsList.Count));
                    break;
                case "Газовый гигант":
                    ObjectsList.ObjectsList.Add(new GasGiant(NewObjectName, SytemCenter, ObjectsList.ObjectsList.Count));
                    break;
                case "Астероид":
                    ObjectsList.ObjectsList.Add(new Asteroid(NewObjectName, SytemCenter, ObjectsList.ObjectsList.Count));
                    break;
                case "Комета":
                    ObjectsList.ObjectsList.Add(new Comet(NewObjectName, SytemCenter, ObjectsList.ObjectsList.Count));
                    break;  
            }
            BuildSystemModel(ObjectsList, DrawingField);
        }

       

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            ObjectsList.Remove(ObjectInFocus);
            BuildSystemModel(ObjectsList, DrawingField);
        }

        private void tbNewObjName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }       

        private void buttonAdd_Copy1_Click(object sender, RoutedEventArgs e)
        {
            OpenSaveDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
}
