using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGen.UI.Controls
{
    public class NumericEntry : Entry
    {
        static NumericEntry()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NumericEntryCustomization", (handler, view) =>
                                                                                           {
                                                                                               if (view is NumericEntry entry)
                                                                                               {
#if ANDROID
#elif IOS || MACCATALYST
#elif WINDOWS
                                                                                                   (entry.Handler?.PlatformView as TextBox)!.BeforeTextChanging += (s, args) =>
                                                                                                                                    {
                                                                                                                                        if (!Regex.IsMatch(args.NewText,
                                                                                                                                                           @"^-?\d*$"))
                                                                                                                                        {
                                                                                                                                            args.Cancel = true;
                                                                                                                                        }
                                                                                                                                    };
#endif
                                                                                               }
                                                                                           });
        }
    }
}
