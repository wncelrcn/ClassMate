using Android.Graphics.Drawables;
using Android.Widget;

namespace IT123P_FinalMP
{
    // Class to apply rounded corners to buttons and layouts
    public static class Styler
    {
        // Apply rounded corners to buttons
        public static void ApplyRoundedCorners(Button button)
        {
            button.SetBackgroundResource(Resource.Drawable.rounded_buttons);
            
        }

        // Apply rounded corners to layouts
        public static void ApplyRoundedCorners(LinearLayout layout, int color)
        {
            layout.SetBackgroundResource(Resource.Drawable.rounded_layout);

            GradientDrawable background = (GradientDrawable)layout.Background;
            background.SetColor(color);

        }
    }
}
