using System.Windows.Threading;

namespace WpfApp1.Utils;

public static class TimerUtils
{
	public static DispatcherTimer CreateTimer(Action callback, TimeSpan interval)
	{
		var timer = new DispatcherTimer { Interval = interval };
		timer.Tick += (s, e) => callback();
		timer.Start();
		return timer;
	}
}
