# Custom Sensor Wrapper - Implementation Summary

## Files Created

### Core Logic (CapFrameX.Sensor)
1. **CustomSensorEntry.cs** - Model for a custom sensor definition
2. **CustomSensorEntryWrapper.cs** - MVVM wrapper with INotifyPropertyChanged
3. **CustomSensorConfig.cs** - Manages persistence and CRUD operations for custom sensors
4. **FormulaEvaluator.cs** - Evaluates mathematical formulas with sensor value substitution

### Contracts (CapFrameX.Contracts)
5. **ICustomSensorEntry.cs** - Interface defining custom sensor properties

### Documentation
6. **CUSTOM_SENSORS.md** - User documentation and usage guide

## Files Modified

### Backend Integration
1. **SensorService.cs**
   - Added `_customSensorConfig` field
   - Updated constructor to accept `IPathService` and initialize CustomSensorConfig
   - Modified `LogCurrentValues()` to calculate and log custom sensor values during capture

2. **CapFrameX.Sensor.csproj**
   - Added new .cs files to compilation list

3. **CapFrameX.Contracts.csproj**
   - Added ICustomSensorEntry.cs to compilation list

### UI/ViewModel
4. **SensorViewModel.cs**
   - Added `_customSensorConfig` field
   - Added `CustomSensors` ObservableCollection
   - Added `SelectedCustomSensor` and `SelectedCustomSensorIndex` properties
   - Added commands: `AddCustomSensorCommand`, `RemoveCustomSensorCommand`, `SaveCustomSensorsCommand`
   - Implemented: `LoadCustomSensors()`, `OnAddCustomSensor()`, `OnRemoveCustomSensor()`, `OnSaveCustomSensors()`

5. **SensorView.xaml**
   - Added third column to grid layout (460px width)
   - Added new Border/DockPanel for "Custom sensors" panel
   - Added DataGrid for custom sensor management with columns: Name, Formula, Unit, Active
   - Added control buttons: Add (+), Remove (-), Save
   - Included tooltip for Formula column explaining syntax

## Key Features Implemented

### 1. Formula Evaluation
- Parse formulas with `[Sensor Name]` references
- Case-insensitive partial matching for sensor names
- Support for +, -, *, / operators and parentheses
- Uses System.Data.DataTable.Compute() for safe evaluation
- Returns false for invalid formulas or divide-by-zero

### 2. Real-time Calculation
- Custom sensors calculated during capture in `LogCurrentValues()`
- Values logged alongside regular sensors
- Integrated into existing SessionSensorData2 structure
- Uses "Custom" hardware type and "Custom Sensors" hardware name

### 3. Persistence
- Configuration stored in `CustomSensorConfiguration.json`
- Located in standard config folder (alongside SensorEntryConfiguration.json)
- JSON serialization with Newtonsoft.Json
- Auto-loads on application startup

### 4. UI Management
- MVVM pattern with commands and data binding
- Editable DataGrid for inline editing
- Material Design UI consistency
- Add/Remove/Save operations with proper validation

### 5. Integration Points
- Custom sensors appear in sensor reports
- Can be used in overlays (same infrastructure as regular sensors)
- Available in post-capture analysis
- Follows existing sensor logging enable/disable patterns

## Architecture Decisions

### Why DataTable.Compute()?
- Built-in, safe mathematical expression evaluator
- No external dependencies
- Handles operator precedence correctly
- Returns typed results

### Why Custom Identifier Format?
- Uses "Custom/{Name}" format to avoid conflicts
- Easily distinguishable from hardware sensors
- Allows future expansion of custom sensor types

### Why Separate Config File?
- Independent lifecycle from hardware sensors
- User-defined data separate from detected hardware
- Easier backup/restore of custom formulas
- No risk of corruption affecting hardware sensor config

## Testing Recommendations

1. **Formula Validation**
   - Test with various mathematical expressions
   - Verify sensor name matching (partial, case-insensitive)
   - Test division by zero handling
   - Verify invalid formulas are gracefully rejected

2. **UI Functionality**
   - Add/Remove custom sensors
   - Edit inline in DataGrid
   - Save/Load persistence
   - Selection and command enable states

3. **Integration**
   - Verify custom sensor values logged during capture
   - Check custom sensors appear in reports
   - Test overlay display (if implemented)
   - Confirm no performance impact during logging

4. **Edge Cases**
   - Empty formula
   - Non-existent sensor references
   - Duplicate custom sensor names
   - Very long formulas
   - Special characters in sensor names

## Future Enhancement Ideas

1. **Formula Builder UI**
   - Dropdown to select sensors
   - Insert operator buttons
   - Real-time preview of calculated value

2. **Advanced Functions**
   - Math functions: sqrt, pow, abs, min, max
   - Aggregations: avg over time window
   - Conditional expressions: if-then-else

3. **Validation Feedback**
   - Show formula errors in UI
   - Highlight invalid sensor references
   - Display calculated preview value

4. **Templates**
   - Pre-defined useful formulas
   - Import/Export custom sensor sets
   - Share formulas with community

5. **Units & Formatting**
   - Proper unit validation
   - Decimal places configuration
   - Scientific notation support
