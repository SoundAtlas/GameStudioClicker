using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GameStudioClicker.Wpf.Controls
{
    public class ClickDragSlider : Slider
    {
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Track? track = Template.FindName("PART_Track", this) as Track;

            // If the user clicks on the track (not the thumb), we want to move the thumb to that position and start dragging.
            bool isTrackClick =
                IsMoveToPointEnabled &&
                track?.Thumb != null &&
                !track.Thumb.IsMouseOver;

            base.OnPreviewMouseLeftButtonDown(e);

            if (isTrackClick &&
                track?.Thumb is Thumb thumb &&
                !thumb.IsDragging)
            {
                UpdateLayout();

                MouseButtonEventArgs thumbMouseEvent =
                    new MouseButtonEventArgs(
                        e.MouseDevice,
                        e.Timestamp,
                        MouseButton.Left)
                    {
                        RoutedEvent =
                        MouseLeftButtonDownEvent
                    };

                thumb.RaiseEvent(thumbMouseEvent);
            }
        }
    }
}
