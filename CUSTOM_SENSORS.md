# Custom Sensor Wrapper Feature

## Overview
Users can now create custom sensor wrappers in the SENSOR Tab. Each custom sensor is defined by a formula that uses existing sensor values to calculate new derived values.

## Use Cases
- Display GPU Clock Speed as a percentage of maximum
- Calculate custom performance indices
- Create power efficiency metrics (e.g., FPS per Watt)
- Any mathematical combination of existing sensors

## How to Use

### Creating a Custom Sensor
1. Navigate to the **SENSOR** tab in CapFrameX
2. Look for the **Custom sensors** panel on the right side
3. Click the **+** button to add a new custom sensor
4. Fill in the following fields:
   - **Name**: A descriptive name for your custom sensor (e.g., "GPU Clock %")
   - **Formula**: A mathematical expression using sensor references (see below)
   - **Unit**: The unit of measurement (e.g., "%", "fps/W", "score")
   - **Active**: Check to enable logging for this custom sensor

### Formula Syntax
Formulas use square brackets `[Sensor Name]` to reference existing sensors. You can use standard mathematical operators.

#### Examples:

**GPU Clock as Percentage:**
```
[GPU Core] * 100 / 2500
```
(Divides GPU Core clock by maximum clock of 2500 MHz)

**Power Efficiency:**
```
[FPS] / [GPU Power]
```
(FPS per Watt)

**Custom Performance Index:**
```
([FPS] * 100) / ([CPU Package] + [GPU Power])
```
(Weighted performance score)

**Temperature Delta:**
```
[GPU Core] - [CPU Package]
```
(Temperature difference between GPU and CPU)

### Supported Operators
- Addition: `+`
- Subtraction: `-`
- Multiplication: `*`
- Division: `/`
- Parentheses: `()` for grouping

### Managing Custom Sensors
- **Add**: Click the **+** button to create a new custom sensor
- **Remove**: Select a sensor and click the **-** button to delete it
- **Save**: Click the **save icon** to persist your custom sensors to disk
- **Edit**: Click on any cell to edit the Name, Formula, Unit, or Active status

### Data Availability
Once saved and activated:
- Custom sensor values are calculated in real-time during capture
- Values are logged alongside standard sensors
- Data appears in the **Sensor statistics** panel after benchmark
- Can be displayed in the overlay during gameplay/benchmarking
- Available in post-capture analysis and reports

## Technical Details

### Architecture
- **CustomSensorEntry**: Defines a custom sensor with name, formula, unit, and active status
- **CustomSensorConfig**: Manages persistence of custom sensors to `CustomSensorConfiguration.json`
- **FormulaEvaluator**: Evaluates formulas at runtime using sensor values
- **Integration**: Custom sensors are calculated during `LogCurrentValues` in `SensorService`

### Configuration File
Custom sensors are stored in:
```
<ConfigFolder>/CustomSensorConfiguration.json
```

Example configuration:
```json
[
  {
    "Name": "GPU Clock %",
    "Formula": "[GPU Core] * 100 / 2500",
    "Unit": "%",
    "IsActive": true
  },
  {
    "Name": "Power Efficiency",
    "Formula": "[FPS] / [GPU Power]",
    "Unit": "fps/W",
    "IsActive": true
  }
]
```

### Sensor Name Matching
The formula evaluator uses case-insensitive partial matching for sensor names. For example, `[GPU Core]` will match sensors containing "GPU Core" in their name.

## Troubleshooting

### Formula Not Working
- Ensure sensor names are enclosed in square brackets `[]`
- Check that referenced sensors exist in your sensor list
- Verify mathematical operators are correct
- Make sure there are no division by zero scenarios

### Custom Sensor Not Logging
- Verify the **Active** checkbox is enabled
- Ensure you clicked the **save icon** after making changes
- Check that **Log sensors** is enabled in the Sensor tab
- Restart logging/capture after making changes

## Future Enhancements
Potential improvements for future versions:
- Formula validation with real-time error feedback
- Support for mathematical functions (sin, cos, sqrt, etc.)
- Conditional logic (if-then-else)
- Historical references (previous frame values)
- Auto-suggest for sensor names while typing formulas
