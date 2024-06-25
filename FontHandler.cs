using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123P_FinalMP
{
    internal class FontHandler
    {
        private Context context;
        private string fontType;

        public FontHandler(Context context, string fontType)
        {
            this.context = context;
            this.fontType = "Fonts/" + fontType;
        }

        public void SetFont(TextView textView)
        {
            Typeface tf = Typeface.CreateFromAsset(context.Assets, fontType);
            textView.SetTypeface(tf, TypefaceStyle.Normal);
        }
    }
}
