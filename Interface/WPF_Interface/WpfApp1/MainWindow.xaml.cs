using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Wpf_Interface.Constants;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly dynamic configurations = JsonConvert.DeserializeObject(ConfigurationConstants.ConfigurationString);
        public MainWindow()
        {
            InitializeComponent();
            ComboBoxVehicleType.ItemsSource = configurations.ToObject<Dictionary<string, object>>().Keys;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected vehicle type
            string selectedVehicle = ComboBoxVehicleType.SelectedItem?.ToString();
            if (selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle type.");
                return;
            }

            // Build parts dictionary dynamically
            var parts = new Dictionary<string, string>();

            // Add parts based on the selected vehicle
            if (selectedVehicle == "etk800")
            {
                // Get selected items
                string engine = ComboBoxEngine.SelectedItem?.ToString();
                string transmission = ComboBoxTransmission.SelectedItem?.ToString();
                string strutWide = ComboBoxStrut.SelectedItem?.ToString();
                string shockWide = ComboBoxShock.SelectedItem?.ToString();
                string strut = ComboBoxStrut.SelectedItem?.ToString();
                string shock = ComboBoxShock.SelectedItem?.ToString();

                // Validate wide selections
                bool isStrutWide = strutWide?.Contains("wide") ?? false;
                bool isShockWide = shockWide?.Contains("wide") ?? false;

                if (isStrutWide && !isShockWide)
                {
                    MessageBox.Show("Please select a wide shock if a wide strut is selected.");
                    return;
                }

                if (isShockWide && !isStrutWide)
                {
                    MessageBox.Show("Please select a wide strut if a wide shock is selected.");
                    return;
                }

                // Build parts dictionary based on selections
                parts.Add("etk_engine", engine);
                parts.Add("etk_transmission", transmission);

                if (isStrutWide && isShockWide)
                {
                    parts.Add("etk800_strut_F_wide", strutWide);
                    parts.Add("etk800_shock_R_wide", shockWide);
                }
                else
                {
                    parts.Add("etk800_strut_F", strut);
                    parts.Add("etk800_shock_R", shock);
                }

                parts.Add("etk800_brake_F", ComboBoxBreakF.SelectedItem?.ToString());
                parts.Add("etk800_brake_R", ComboBoxBreakR.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "van")
            {
                parts.Add("van_engine", ComboBoxEngine.SelectedItem?.ToString());
                parts.Add("van_transmission", ComboBoxTransmission.SelectedItem?.ToString());
                parts.Add("van_suspension_F", ComboBoxStrut.SelectedItem?.ToString());
                parts.Add("van_suspension_R", ComboBoxShock.SelectedItem?.ToString());
                parts.Add("van_brake_F", ComboBoxBreakF.SelectedItem?.ToString());
                parts.Add("van_brake_R", ComboBoxBreakR.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "pickup")
            {
                parts.Add("pickup_engine", ComboBoxEngine.SelectedItem?.ToString());
                parts.Add("pickup_transmission", ComboBoxTransmission.SelectedItem?.ToString());
                parts.Add("pickup_suspension_F", ComboBoxStrut.SelectedItem?.ToString());
                parts.Add("pickup_suspension_R", ComboBoxShock.SelectedItem?.ToString());
                parts.Add("pickup_brake_F", ComboBoxBreakF.SelectedItem?.ToString());
                parts.Add("pickup_brake_R", ComboBoxBreakR.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "covet")
            {
                parts.Add("covet_engine", ComboBoxEngine.SelectedItem?.ToString());
                parts.Add("covet_transmission", ComboBoxTransmission.SelectedItem?.ToString());
                parts.Add("covet_coilover_F", ComboBoxStrut.SelectedItem?.ToString());
                parts.Add("covet_coilover_R", ComboBoxShock.SelectedItem?.ToString());
                parts.Add("covet_brake_F", ComboBoxBreakF.SelectedItem?.ToString());
                parts.Add("covet_brake_R", ComboBoxBreakR.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "bolide")
            {
                parts.Add("bolide_engine", ComboBoxEngine.SelectedItem?.ToString());
                parts.Add("bolide_coilover_F", ComboBoxStrut.SelectedItem?.ToString());
                parts.Add("bolide_coilover_R", ComboBoxShock.SelectedItem?.ToString());
                parts.Add("bolide_brake_F", ComboBoxBreakF.SelectedItem?.ToString());
                parts.Add("bolide_brake_R", ComboBoxBreakR.SelectedItem?.ToString());
            }

            // Create the final JSON object
            var vehicleConfig = new
            {
                vehicle = selectedVehicle,
                parts = parts
            };

            // Write JSON to file
            string jsonOutput = JsonConvert.SerializeObject(vehicleConfig, Formatting.Indented);
            File.WriteAllText(@"..\..\..\..\..\..\beamng.tech integration\vehicle_config.json", jsonOutput);

            // Optional: Display a confirmation
            MessageBox.Show("Configuration saved to vehicle_config.json");

            // Run Python script
            RunPythonScript();
        }

        private void ComboBoxVehicleType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get selected vehicle type
            string selectedVehicle = ComboBoxVehicleType.SelectedItem?.ToString();
            if (selectedVehicle == null || !configurations.ContainsKey(selectedVehicle)) return;

            List<string> vehicles = new List<string>();

            // Retrieve the configuration for the selected vehicle
            var vehicleConfig = configurations[selectedVehicle].config;

            FillComboBoxes(vehicleConfig, selectedVehicle);
        }

        private void FillComboBoxes(dynamic vehicleConfig, string selectedVehicle)
        {
            switch (selectedVehicle)
            {
                case "etk800":
                    ComboBoxEngine.ItemsSource = vehicleConfig["etk_engine"].ToObject<List<string>>();
                    ComboBoxTransmission.ItemsSource = vehicleConfig["etk_transmission"].ToObject<List<string>>();
                    ComboBoxStrut.ItemsSource =
                        ((IEnumerable<string>)vehicleConfig["etk800_strut_F"].ToObject<List<string>>())
                        .Concat((IEnumerable<string>)vehicleConfig["etk800_strut_F_wide"].ToObject<List<string>>())
                        .ToList();  //I'm sorry for this
                    ComboBoxShock.ItemsSource =
                        ((IEnumerable<string>)vehicleConfig["etk800_shock_R"].ToObject<List<string>>())
                        .Concat((IEnumerable<string>)vehicleConfig["etk800_shock_R_wide"].ToObject<List<string>>())
                        .ToList();  //sorry for this as well :3
                    ComboBoxBreakF.ItemsSource = vehicleConfig["etk800_brake_F"].ToObject<List<string>>();
                    ComboBoxBreakR.ItemsSource = vehicleConfig["etk800_brake_R"].ToObject<List<string>>();
                    break;
                case "van":
                    ComboBoxEngine.ItemsSource = vehicleConfig["van_engine"].ToObject<List<string>>();
                    ComboBoxTransmission.ItemsSource = vehicleConfig["van_transmission"].ToObject<List<string>>();
                    ComboBoxStrut.ItemsSource = vehicleConfig["van_suspension_F"].ToObject<List<string>>();
                    ComboBoxShock.ItemsSource = vehicleConfig["van_suspension_R"].ToObject<List<string>>();
                    ComboBoxBreakF.ItemsSource = vehicleConfig["van_brake_F"].ToObject<List<string>>();
                    ComboBoxBreakR.ItemsSource = vehicleConfig["van_brake_R"].ToObject<List<string>>();
                    break;
                case "pickup":
                    ComboBoxEngine.ItemsSource = vehicleConfig["pickup_engine"].ToObject<List<string>>();
                    ComboBoxTransmission.ItemsSource = vehicleConfig["pickup_transmission"].ToObject<List<string>>();
                    ComboBoxStrut.ItemsSource = vehicleConfig["pickup_suspension_F"].ToObject<List<string>>();
                    ComboBoxShock.ItemsSource = vehicleConfig["pickup_suspension_R"].ToObject<List<string>>();
                    ComboBoxBreakF.ItemsSource = vehicleConfig["pickup_brake_F"].ToObject<List<string>>();
                    ComboBoxBreakR.ItemsSource = vehicleConfig["pickup_brake_R"].ToObject<List<string>>();
                    break;
                case "covet":
                    ComboBoxEngine.ItemsSource = vehicleConfig["covet_engine"].ToObject<List<string>>();
                    ComboBoxTransmission.ItemsSource = vehicleConfig["covet_transmission"].ToObject<List<string>>();
                    ComboBoxStrut.ItemsSource = vehicleConfig["covet_coilover_F"].ToObject<List<string>>();
                    ComboBoxShock.ItemsSource = vehicleConfig["covet_coilover_R"].ToObject<List<string>>();
                    ComboBoxBreakF.ItemsSource = vehicleConfig["covet_brake_F"].ToObject<List<string>>();
                    ComboBoxBreakR.ItemsSource = vehicleConfig["covet_brake_R"].ToObject<List<string>>();
                    break;
                case "bolide":
                    ComboBoxEngine.ItemsSource = vehicleConfig["bolide_engine"].ToObject<List<string>>();
                    ComboBoxTransmission.ItemsSource = vehicleConfig["bolide_coilover_F"].ToObject<List<string>>();
                    ComboBoxStrut.ItemsSource = vehicleConfig["bolide_coilover_F"].ToObject<List<string>>();
                    ComboBoxShock.ItemsSource = vehicleConfig["bolide_coilover_R"].ToObject<List<string>>();
                    ComboBoxBreakF.ItemsSource = vehicleConfig["bolide_brake_F"].ToObject<List<string>>();
                    ComboBoxBreakR.ItemsSource = vehicleConfig["bolide_brake_R"].ToObject<List<string>>();
                    break;
                default:
                    break;
            }
        }

        private static void RunPythonScript()
        {
            string scriptPath = @"..\..\..\..\..\..\beamng.tech integration\beamng_track_simulator.py";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"D:\\BeamNG.tech.v0.33.3.0\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Redirect standard error to capture errors
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                var process = System.Diagnostics.Process.Start(psi);

                // Read output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Display output or errors
                if (!output.Contains("Total Time to Complete Track:"))
                {
                    MessageBox.Show($"Error: {error}", "Script Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var time = output.Split("Total Time to Complete Track:")[1].Trim();
                    MessageBox.Show($"The car completed the track in {time}", "Test ran", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to run Python script: {ex.Message}", "Execution Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}