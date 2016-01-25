# multi-tap-behavior
XAML Behavior that allows reacting to multiple tap-events, for WinRT (additional platforms to be added)

## Usage:

### Namespace reference:

    xmlns:interactivity="using:Microsoft.Xaml.Interactivity"
    xmlns:core="using:Microsoft.Xaml.Interactions.Core"

### Triggering Commands (with optional CommandParameter):

    <AnyUIElement>
    	<interactivity:Interaction.Behaviors>
    		<helpers:MultiTapBehavior TapCount="5" Command="{Binding SomeCommand}" CommandParameter="optional_parameter_value" />
    	</interactivity:Interaction.Behaviors>
    </AnyUIElement>

### Triggering any other actions:

    <AnyUIElement>
    	<interactivity:Interaction.Behaviors>
    		<helpers:MultiTapBehavior TapCount="5">
    		    <core:InvokeCommandAction Command="{Binding SomeCommand}" />
                <core:CallMethodAction MethodName="MyMethod" TargetObject="{Binding SomeObject}"/>
                <core:NavigateToPageAction TargetPage="SomePage"/>
                ...
    		</helpers:MultiTapBehavior TapCount="5">
    	</interactivity:Interaction.Behaviors>
    </AnyUIElement>
