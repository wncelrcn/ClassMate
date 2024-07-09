using Android.Graphics.Drawables;
using Android.Widget;

namespace IT123P_FinalMP
{
    public static class Styler
    {
        public static void ApplyRoundedCorners(Button button)
        {
            button.SetBackgroundResource(Resource.Drawable.rounded_buttons);
            
        }

        public static void ApplyRoundedCorners(LinearLayout layout, int color)
        {
            layout.SetBackgroundResource(Resource.Drawable.rounded_layout);

            GradientDrawable background = (GradientDrawable)layout.Background;
            background.SetColor(color);

        }
    }
}
