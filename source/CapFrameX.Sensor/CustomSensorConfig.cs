using CapFrameX.Contracts.Sensor;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CapFrameX.Sensor
{
    public class CustomSensorConfig
    {
        private static readonly string CONFIG_FILENAME = "CustomSensorConfiguration.json";
        private readonly string _configFolder;
        private List<CustomSensorEntry> _customSensors;

        public CustomSensorConfig(string configFolder)
        {
            _configFolder = configFolder;
            _customSensors = new List<CustomSensorEntry>();
            Task.Run(async () => await LoadOrSetDefault()).Wait();
        }

        public IEnumerable<CustomSensorEntry> GetCustomSensors()
        {
            return _customSensors.ToList();
        }

        public void AddCustomSensor(CustomSensorEntry sensor)
        {
            if (_customSensors.Any(s => s.Name == sensor.Name))
            {
                throw new ArgumentException($"Custom sensor with name '{sensor.Name}' already exists.");
            }
            _customSensors.Add(sensor);
        }

        public void UpdateCustomSensor(string originalName, CustomSensorEntry updatedSensor)
        {
            var existing = _customSensors.FirstOrDefault(s => s.Name == originalName);
            if (existing != null)
            {
                existing.Name = updatedSensor.Name;
                existing.Formula = updatedSensor.Formula;
                existing.Unit = updatedSensor.Unit;
                existing.IsActive = updatedSensor.IsActive;
            }
        }

        public void RemoveCustomSensor(string name)
        {
            _customSensors.RemoveAll(s => s.Name == name);
        }

        public async Task Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_customSensors, Formatting.Indented);

                if (!Directory.Exists(_configFolder))
                    Directory.CreateDirectory(_configFolder);

                var filePath = Path.Combine(_configFolder, CONFIG_FILENAME);
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    await outputFile.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error while saving custom sensor config.");
            }
        }

        private async Task LoadOrSetDefault()
        {
            try
            {
                var filePath = Path.Combine(_configFolder, CONFIG_FILENAME);
                if (!File.Exists(filePath))
                {
                    _customSensors = new List<CustomSensorEntry>();
                    return;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    var json = await reader.ReadToEndAsync();
                    _customSensors = JsonConvert.DeserializeObject<List<CustomSensorEntry>>(json) 
                        ?? new List<CustomSensorEntry>();
                }
            }
            catch (Exception ex)
            {
                _customSensors = new List<CustomSensorEntry>();
                Log.Logger.Error(ex, "Error while loading custom sensor config.");
            }
        }
    }
}
