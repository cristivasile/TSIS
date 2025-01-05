using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Wpf_Interface.Constants;
using WpfApp1;

namespace Wpf_Interface
{
    /// <summary>
    /// Interaction logic for CompareConfigsWindow.xaml
    /// </summary>
    public partial class CompareConfigsWindow : Window
    {
        public readonly dynamic configurations = JsonConvert.DeserializeObject(ConfigurationConstants.ConfigurationString);
        public CompareConfigsWindow()
        {
            // Check if the Python script exists
            if (!Directory.Exists(@"D:\BeamNG.tech.v0.33.3.0"))
            {
                MessageBox.Show($"Beamng.tech not installed at the expected path!",
                                    "Missing File", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // Stop the program
                return;
            }

            string scriptPath = @"..\beamng_track_simulator.py";
            if (!File.Exists(scriptPath))
            {
                MessageBox.Show($"The required file '{Path.GetFileName(scriptPath)}' was not found in the current folder!",
                                "Missing File", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // Stop the program
                return;
            }

            InitializeComponent();
            ComboBoxVehicleType1.ItemsSource = configurations.ToObject<Dictionary<string, object>>().Keys;
            ComboBoxVehicleType2.ItemsSource = configurations.ToObject<Dictionary<string, object>>().Keys;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected vehicle type
            string selectedVehicle = ComboBoxVehicleType1.SelectedItem?.ToString();
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
                string engine = ComboBoxEngine1.SelectedItem?.ToString();
                string transmission = ComboBoxTransmission1.SelectedItem?.ToString();
                string strutWide = ComboBoxStrut1.SelectedItem?.ToString();
                string shockWide = ComboBoxShock1.SelectedItem?.ToString();
                string strut = ComboBoxStrut1.SelectedItem?.ToString();
                string shock = ComboBoxShock1.SelectedItem?.ToString();

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

                parts.Add("etk800_brake_F", ComboBoxBreakF1.SelectedItem?.ToString());
                parts.Add("etk800_brake_R", ComboBoxBreakR1.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "van")
            {
                parts.Add("van_engine", ComboBoxEngine1.SelectedItem?.ToString());
                parts.Add("van_transmission", ComboBoxTransmission1.SelectedItem?.ToString());
                parts.Add("van_suspension_F", ComboBoxStrut1.SelectedItem?.ToString());
                parts.Add("van_suspension_R", ComboBoxShock1.SelectedItem?.ToString());
                parts.Add("van_brake_F", ComboBoxBreakF1.SelectedItem?.ToString());
                parts.Add("van_brake_R", ComboBoxBreakR1.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "pickup")
            {
                parts.Add("pickup_engine", ComboBoxEngine1.SelectedItem?.ToString());
                parts.Add("pickup_transmission", ComboBoxTransmission1.SelectedItem?.ToString());
                parts.Add("pickup_suspension_F", ComboBoxStrut1.SelectedItem?.ToString());
                parts.Add("pickup_suspension_R", ComboBoxShock1.SelectedItem?.ToString());
                parts.Add("pickup_brake_F", ComboBoxBreakF1.SelectedItem?.ToString());
                parts.Add("pickup_brake_R", ComboBoxBreakR1.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "covet")
            {
                parts.Add("covet_engine", ComboBoxEngine1.SelectedItem?.ToString());
                parts.Add("covet_transmission", ComboBoxTransmission1.SelectedItem?.ToString());
                parts.Add("covet_coilover_F", ComboBoxStrut1.SelectedItem?.ToString());
                parts.Add("covet_coilover_R", ComboBoxShock1.SelectedItem?.ToString());
                parts.Add("covet_brake_F", ComboBoxBreakF1.SelectedItem?.ToString());
                parts.Add("covet_brake_R", ComboBoxBreakR1.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "bolide")
            {
                parts.Add("bolide_engine", ComboBoxEngine1.SelectedItem?.ToString());
                parts.Add("bolide_coilover_F", ComboBoxStrut1.SelectedItem?.ToString());
                parts.Add("bolide_coilover_R", ComboBoxShock1.SelectedItem?.ToString());
                parts.Add("bolide_brake_F", ComboBoxBreakF1.SelectedItem?.ToString());
                parts.Add("bolide_brake_R", ComboBoxBreakR1.SelectedItem?.ToString());
            }

            // Create the final JSON object
            var vehicleConfig = new
            {
                vehicle = selectedVehicle,
                parts
            };

            // Write JSON to file
            string jsonOutput = JsonConvert.SerializeObject(vehicleConfig, Formatting.Indented);
            File.WriteAllText(@"..\vehicle_config.json", jsonOutput);

            // Optional: Display a confirmation
            MessageBox.Show("BeamNG will run shortly...");

            // Run Python script
            var time1 = RunPythonScript();






            // Get selected vehicle type
            selectedVehicle = ComboBoxVehicleType2.SelectedItem?.ToString();
            if (selectedVehicle == null)
            {
                MessageBox.Show("Please select a vehicle type.");
                return;
            }

            // Build parts dictionary dynamically
            parts = new Dictionary<string, string>();

            // Add parts based on the selected vehicle
            if (selectedVehicle == "etk800")
            {
                // Get selected items
                string engine = ComboBoxEngine2.SelectedItem?.ToString();
                string transmission = ComboBoxTransmission2.SelectedItem?.ToString();
                string strutWide = ComboBoxStrut2.SelectedItem?.ToString();
                string shockWide = ComboBoxShock2.SelectedItem?.ToString();
                string strut = ComboBoxStrut2.SelectedItem?.ToString();
                string shock = ComboBoxShock2.SelectedItem?.ToString();

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

                parts.Add("etk800_brake_F", ComboBoxBreakF2.SelectedItem?.ToString());
                parts.Add("etk800_brake_R", ComboBoxBreakR2.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "van")
            {
                parts.Add("van_engine", ComboBoxEngine2.SelectedItem?.ToString());
                parts.Add("van_transmission", ComboBoxTransmission2.SelectedItem?.ToString());
                parts.Add("van_suspension_F", ComboBoxStrut2.SelectedItem?.ToString());
                parts.Add("van_suspension_R", ComboBoxShock2.SelectedItem?.ToString());
                parts.Add("van_brake_F", ComboBoxBreakF2.SelectedItem?.ToString());
                parts.Add("van_brake_R", ComboBoxBreakR2.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "pickup")
            {
                parts.Add("pickup_engine", ComboBoxEngine2.SelectedItem?.ToString());
                parts.Add("pickup_transmission", ComboBoxTransmission2.SelectedItem?.ToString());
                parts.Add("pickup_suspension_F", ComboBoxStrut2.SelectedItem?.ToString());
                parts.Add("pickup_suspension_R", ComboBoxShock2.SelectedItem?.ToString());
                parts.Add("pickup_brake_F", ComboBoxBreakF2.SelectedItem?.ToString());
                parts.Add("pickup_brake_R", ComboBoxBreakR2.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "covet")
            {
                parts.Add("covet_engine", ComboBoxEngine2.SelectedItem?.ToString());
                parts.Add("covet_transmission", ComboBoxTransmission2.SelectedItem?.ToString());
                parts.Add("covet_coilover_F", ComboBoxStrut2.SelectedItem?.ToString());
                parts.Add("covet_coilover_R", ComboBoxShock2.SelectedItem?.ToString());
                parts.Add("covet_brake_F", ComboBoxBreakF2.SelectedItem?.ToString());
                parts.Add("covet_brake_R", ComboBoxBreakR2.SelectedItem?.ToString());
            }
            else if (selectedVehicle == "bolide")
            {
                parts.Add("bolide_engine", ComboBoxEngine2.SelectedItem?.ToString());
                parts.Add("bolide_coilover_F", ComboBoxStrut2.SelectedItem?.ToString());
                parts.Add("bolide_coilover_R", ComboBoxShock2.SelectedItem?.ToString());
                parts.Add("bolide_brake_F", ComboBoxBreakF2.SelectedItem?.ToString());
                parts.Add("bolide_brake_R", ComboBoxBreakR2.SelectedItem?.ToString());
            }

            // Create the final JSON object
            vehicleConfig = new
            {
                vehicle = selectedVehicle,
                parts
            };

            // Write JSON to file
            jsonOutput = JsonConvert.SerializeObject(vehicleConfig, Formatting.Indented);
            File.WriteAllText(@"..\vehicle_config.json", jsonOutput);

            // Optional: Display a confirmation
            MessageBox.Show("BeamNG will run shortly...");

            // Run Python script
            var time2 = RunPythonScript();

            MessageBox.Show($"Total Time to Complete Track for first Vehicle: {time1}\n" +
                $"Total Time to Complete Track for second Vehicle: {time2}");
        }

        private void ComboBoxVehicleType1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get selected vehicle type
            string selectedVehicle = ComboBoxVehicleType1.SelectedItem?.ToString();
            if (selectedVehicle == null || !configurations.ContainsKey(selectedVehicle)) return;

            // Enable relevant inputs
            EnableInputs(selectedVehicle, true);

            // Retrieve the configuration for the selected vehicle
            var vehicleConfig = configurations[selectedVehicle].config;

            // Populate combo boxes
            FillComboBoxes(vehicleConfig, selectedVehicle, true);

            // Automatically select the first value for each combo box
            AutoSelectFirstValue(selectedVehicle, true);
        }

        private void ComboBoxVehicleType2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get selected vehicle type
            string selectedVehicle = ComboBoxVehicleType2.SelectedItem?.ToString();
            if (selectedVehicle == null || !configurations.ContainsKey(selectedVehicle)) return;

            // Enable relevant inputs
            EnableInputs(selectedVehicle, false);

            // Retrieve the configuration for the selected vehicle
            var vehicleConfig = configurations[selectedVehicle].config;

            // Populate combo boxes
            FillComboBoxes(vehicleConfig, selectedVehicle, false);

            // Automatically select the first value for each combo box
            AutoSelectFirstValue(selectedVehicle, false);
        }

        private void EnableInputs(string selectedVehicle, bool isLeft)
        {
            if (isLeft)
            {
                ComboBoxEngine1.IsEnabled = true;
                ComboBoxStrut1.IsEnabled = true;
                ComboBoxShock1.IsEnabled = true;
                ComboBoxBreakF1.IsEnabled = true;
                ComboBoxBreakR1.IsEnabled = true;

                // Disable transmission for bolide
                ComboBoxTransmission1.IsEnabled = selectedVehicle != "bolide";
            }
            else
            {
                ComboBoxEngine2.IsEnabled = true;
                ComboBoxStrut2.IsEnabled = true;
                ComboBoxShock2.IsEnabled = true;
                ComboBoxBreakF2.IsEnabled = true;
                ComboBoxBreakR2.IsEnabled = true;

                // Disable transmission for bolide
                ComboBoxTransmission2.IsEnabled = selectedVehicle != "bolide";
            }
        }

        private void AutoSelectFirstValue(string selectedVehicle, bool isLeft)
        {
            if (isLeft)
            {
                ComboBoxEngine1.SelectedIndex = 0;
                ComboBoxStrut1.SelectedIndex = 0;
                ComboBoxShock1.SelectedIndex = 0;
                ComboBoxBreakF1.SelectedIndex = 0;
                ComboBoxBreakR1.SelectedIndex = 0;

                if (selectedVehicle != "bolide")
                {
                    ComboBoxTransmission1.SelectedIndex = 0;
                }
                else
                {
                    ComboBoxTransmission1.SelectedIndex = -1;
                }
            }
            else
            {
                ComboBoxEngine2.SelectedIndex = 0;
                ComboBoxStrut2.SelectedIndex = 0;
                ComboBoxShock2.SelectedIndex = 0;
                ComboBoxBreakF2.SelectedIndex = 0;
                ComboBoxBreakR2.SelectedIndex = 0;

                if (selectedVehicle != "bolide")
                {
                    ComboBoxTransmission2.SelectedIndex = 0;
                }
                else
                {
                    ComboBoxTransmission2.SelectedIndex = -1;
                }
            }
        }


        private void FillComboBoxes(dynamic vehicleConfig, string selectedVehicle, bool isLeft)
        {
            if (isLeft)
            {
                switch (selectedVehicle)
                {
                    case "etk800":
                        ComboBoxEngine1.ItemsSource = vehicleConfig["etk_engine"].ToObject<List<string>>();
                        ComboBoxTransmission1.ItemsSource = vehicleConfig["etk_transmission"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource =
                            ((IEnumerable<string>)vehicleConfig["etk800_strut_F"].ToObject<List<string>>())
                            .Concat((IEnumerable<string>)vehicleConfig["etk800_strut_F_wide"].ToObject<List<string>>())
                            .ToList();  //I'm sorry for this
                        ComboBoxShock1.ItemsSource =
                            ((IEnumerable<string>)vehicleConfig["etk800_shock_R"].ToObject<List<string>>())
                            .Concat((IEnumerable<string>)vehicleConfig["etk800_shock_R_wide"].ToObject<List<string>>())
                            .ToList();  //sorry for this as well :3
                        ComboBoxBreakF1.ItemsSource = vehicleConfig["etk800_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR1.ItemsSource = vehicleConfig["etk800_brake_R"].ToObject<List<string>>();
                        break;
                    case "van":
                        ComboBoxEngine1.ItemsSource = vehicleConfig["van_engine"].ToObject<List<string>>();
                        ComboBoxTransmission1.ItemsSource = vehicleConfig["van_transmission"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource = vehicleConfig["van_suspension_F"].ToObject<List<string>>();
                        ComboBoxShock1.ItemsSource = vehicleConfig["van_suspension_R"].ToObject<List<string>>();
                        ComboBoxBreakF1.ItemsSource = vehicleConfig["van_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR1.ItemsSource = vehicleConfig["van_brake_R"].ToObject<List<string>>();
                        break;
                    case "pickup":
                        ComboBoxEngine1.ItemsSource = vehicleConfig["pickup_engine"].ToObject<List<string>>();
                        ComboBoxTransmission1.ItemsSource = vehicleConfig["pickup_transmission"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource = vehicleConfig["pickup_suspension_F"].ToObject<List<string>>();
                        ComboBoxShock1.ItemsSource = vehicleConfig["pickup_suspension_R"].ToObject<List<string>>();
                        ComboBoxBreakF1.ItemsSource = vehicleConfig["pickup_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR1.ItemsSource = vehicleConfig["pickup_brake_R"].ToObject<List<string>>();
                        break;
                    case "covet":
                        ComboBoxEngine1.ItemsSource = vehicleConfig["covet_engine"].ToObject<List<string>>();
                        ComboBoxTransmission1.ItemsSource = vehicleConfig["covet_transmission"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource = vehicleConfig["covet_coilover_F"].ToObject<List<string>>();
                        ComboBoxShock1.ItemsSource = vehicleConfig["covet_coilover_R"].ToObject<List<string>>();
                        ComboBoxBreakF1.ItemsSource = vehicleConfig["covet_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR1.ItemsSource = vehicleConfig["covet_brake_R"].ToObject<List<string>>();
                        break;
                    case "bolide":
                        ComboBoxEngine1.ItemsSource = vehicleConfig["bolide_engine"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource = vehicleConfig["bolide_coilover_F"].ToObject<List<string>>();
                        ComboBoxShock1.ItemsSource = vehicleConfig["bolide_coilover_R"].ToObject<List<string>>();
                        ComboBoxBreakF1.ItemsSource = vehicleConfig["bolide_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR1.ItemsSource = vehicleConfig["bolide_brake_R"].ToObject<List<string>>();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (selectedVehicle)
                {
                    case "etk800":
                        ComboBoxEngine2.ItemsSource = vehicleConfig["etk_engine"].ToObject<List<string>>();
                        ComboBoxTransmission2.ItemsSource = vehicleConfig["etk_transmission"].ToObject<List<string>>();
                        ComboBoxStrut1.ItemsSource =
                            ((IEnumerable<string>)vehicleConfig["etk800_strut_F"].ToObject<List<string>>())
                            .Concat((IEnumerable<string>)vehicleConfig["etk800_strut_F_wide"].ToObject<List<string>>())
                            .ToList();  //I'm sorry for this
                        ComboBoxShock2.ItemsSource =
                            ((IEnumerable<string>)vehicleConfig["etk800_shock_R"].ToObject<List<string>>())
                            .Concat((IEnumerable<string>)vehicleConfig["etk800_shock_R_wide"].ToObject<List<string>>())
                            .ToList();  //sorry for this as well :3
                        ComboBoxBreakF2.ItemsSource = vehicleConfig["etk800_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR2.ItemsSource = vehicleConfig["etk800_brake_R"].ToObject<List<string>>();
                        break;
                    case "van":
                        ComboBoxEngine2.ItemsSource = vehicleConfig["van_engine"].ToObject<List<string>>();
                        ComboBoxTransmission2.ItemsSource = vehicleConfig["van_transmission"].ToObject<List<string>>();
                        ComboBoxStrut2.ItemsSource = vehicleConfig["van_suspension_F"].ToObject<List<string>>();
                        ComboBoxShock2.ItemsSource = vehicleConfig["van_suspension_R"].ToObject<List<string>>();
                        ComboBoxBreakF2.ItemsSource = vehicleConfig["van_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR2.ItemsSource = vehicleConfig["van_brake_R"].ToObject<List<string>>();
                        break;
                    case "pickup":
                        ComboBoxEngine2.ItemsSource = vehicleConfig["pickup_engine"].ToObject<List<string>>();
                        ComboBoxTransmission2.ItemsSource = vehicleConfig["pickup_transmission"].ToObject<List<string>>();
                        ComboBoxStrut2.ItemsSource = vehicleConfig["pickup_suspension_F"].ToObject<List<string>>();
                        ComboBoxShock2.ItemsSource = vehicleConfig["pickup_suspension_R"].ToObject<List<string>>();
                        ComboBoxBreakF2.ItemsSource = vehicleConfig["pickup_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR2.ItemsSource = vehicleConfig["pickup_brake_R"].ToObject<List<string>>();
                        break;
                    case "covet":
                        ComboBoxEngine2.ItemsSource = vehicleConfig["covet_engine"].ToObject<List<string>>();
                        ComboBoxTransmission2.ItemsSource = vehicleConfig["covet_transmission"].ToObject<List<string>>();
                        ComboBoxStrut2.ItemsSource = vehicleConfig["covet_coilover_F"].ToObject<List<string>>();
                        ComboBoxShock2.ItemsSource = vehicleConfig["covet_coilover_R"].ToObject<List<string>>();
                        ComboBoxBreakF2.ItemsSource = vehicleConfig["covet_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR2.ItemsSource = vehicleConfig["covet_brake_R"].ToObject<List<string>>();
                        break;
                    case "bolide":
                        ComboBoxEngine2.ItemsSource = vehicleConfig["bolide_engine"].ToObject<List<string>>();
                        ComboBoxStrut2.ItemsSource = vehicleConfig["bolide_coilover_F"].ToObject<List<string>>();
                        ComboBoxShock2.ItemsSource = vehicleConfig["bolide_coilover_R"].ToObject<List<string>>();
                        ComboBoxBreakF2.ItemsSource = vehicleConfig["bolide_brake_F"].ToObject<List<string>>();
                        ComboBoxBreakR2.ItemsSource = vehicleConfig["bolide_brake_R"].ToObject<List<string>>();
                        break;
                    default:
                        break;
                }
            }
        }

        private static string RunPythonScript()
        {
            string scriptPath = @"..\beamng_track_simulator.py";
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
                    return "";
                }
                else
                {
                    var time = output.Split("Total Time to Complete Track:")[1].Trim();
                    return time;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to run Python script: {ex.Message}", "Execution Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }
    }
}
