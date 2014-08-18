using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace tursibNow.AndroidUI
{
    static class Utils
    {
        public static string GetStringResource(this Context context, int id)
        {
            Android.Content.Res.Resources res = context.Resources;
            return res.GetString(id);
        }
    }
}