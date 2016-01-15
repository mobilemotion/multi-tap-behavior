# multi-tap-behavior
XAML Behavior that allows reacting to multiple tap-events, for WinRT (additional platforms to be added)

## Usage:

    <Image Source="somepic.png" IsDoubleTapEnabled="False">
    	<interactivity:Interaction.Behaviors>
    		<helpers:MultiTapBehavior TapCount="5" Command="{Binding SomeCommand}" CommandParameter="optional_parameter_value" />
    	</interactivity:Interaction.Behaviors>
    </Image>
