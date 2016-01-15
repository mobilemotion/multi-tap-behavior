using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

/// <summary>
/// XAML Behaviour that executes a command after a given number of taps (similar to douple-tap).
/// </summary>
public class MultiTapBehavior : DependencyObject, IBehavior
{
	#region Behavior

	/// <summary>
	/// When attaching the behavior to any visual element, we need to register to this elemets'
	/// Tapped and DoubleTapped events.
	/// </summary>
	/// <param name="associatedObject"></param>
	public void Attach(DependencyObject associatedObject)
	{
		var el = associatedObject as FrameworkElement;
		if (el != null)
		{
			el.Tapped += ElementOnTapped;
			el.DoubleTapped += ElementOnDoubleTapped;
		}
	}

	/// <summary>
	/// When deattaching the behavior from a visual element, unregister Tapped and DoubleTapped
	/// event handlers.
	/// </summary>
	public void Detach()
	{
		var el = AssociatedObject as FrameworkElement;
		if (el != null)
		{
			el.Tapped -= ElementOnTapped;
			el.DoubleTapped -= ElementOnDoubleTapped;
		}
	}

	public DependencyObject AssociatedObject { get; private set; }

	#endregion

	#region TapCount property

	public static readonly DependencyProperty TapCountProperty = DependencyProperty.Register(
		"TapCount", typeof (int), typeof (MultiTapBehavior), new PropertyMetadata(3));

	public int TapCount
	{
		get { return (int) GetValue(TapCountProperty); }
		set { SetValue(TapCountProperty, value); }
	}

	#endregion

	#region Command property

	public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
		"Command", typeof(ICommand), typeof(MultiTapBehavior), new PropertyMetadata(default(ICommand)));

	public ICommand Command
	{
		get { return (ICommand) GetValue(CommandProperty); }
		set { SetValue(CommandProperty, value); }
	}

	#endregion

	#region CommandParameter property

	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
		"CommandParameter", typeof(object), typeof(MultiTapBehavior), new PropertyMetadata(null));

	public object CommandParameter
	{
		get { return GetValue(CommandParameterProperty); }
		set { SetValue(CommandParameterProperty, value); }
	}

	#endregion

	#region Event handling

	private DateTime? _previousTapTime = null;
	private int _currentTapCount = 0;

	/// <summary>
	/// On each Tap event, the tap has to be registered in our internal tap counter.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="tappedRoutedEventArgs"></param>
	private void ElementOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
	{
		RegisterTap();
	}

	/// <summary>
	/// In Store Apps, when registering to both Tapped and DoubleTapped event of an element, both
	/// events are fired after double-clicking the element. This means, the DoubleTapped event
	/// handler can assume that the Tapped handler has been fired already, and only one additional
	/// tap needs to be registered in our internal tap counter.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="doubleTappedRoutedEventArgs"></param>
	private void ElementOnDoubleTapped(object sender, DoubleTappedRoutedEventArgs doubleTappedRoutedEventArgs)
	{
		RegisterTap();
	}

	/// <summary>
	/// On each tap, we need to check whether another tap event has been registered within 2
	/// seconds, and if the total number of registered taps meets the desired TapCount property.
	/// </summary>
	private void RegisterTap()
	{
		var now = DateTime.Now;
		if (!_previousTapTime.HasValue)
		{
			// This is the first tap ever
			_previousTapTime = now;
			_currentTapCount = 1;
		}
		else
		{
			// Two taps must occur within 2 seconds to be counted as part of a multi-tap gesture
			if ((now - _previousTapTime).Value.Seconds <= 2)
			{
				_previousTapTime = now;
				_currentTapCount += 1;
			}
			else
			{
				_previousTapTime = now;
				_currentTapCount = 1;
			}
		}

		if (_currentTapCount >= TapCount)
		{
			// If TapCount threshold is met, execute the given Command...
			if (Command != default(ICommand))
			{
				if (Command.CanExecute(CommandParameter))
					Command.Execute(CommandParameter);
			}

			// ...and reset the tap counter
			_previousTapTime = null;
			_currentTapCount = 0;
		}
	}

	#endregion
}
