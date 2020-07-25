using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MSHC.WPF.Controls
{
    /**
     * 
     * Grid with fast way to specify RowDefinitions/ColumnDefinitions:
     * 
     *    FixRowDefinitions="* * Auto 123 Auto"
     * or FixRowDefinitions="1* 4* auto"
     * or FixRowDefinitions="(Height=123, MinHeight=2, MaxHeight=999) (Height=33) Auto (*, SharedSizeGroup=MyGroup)"
     * or FixRowDefinitions="(Len=Auto, SSG=MyGroup) * (Len=Auto, SSG=MyGroup, Min=100)"
     * 
     * (same for FixColumnDefinitions, with /s/Height/Width/g )
     * 
     **/
    public class FixGrid: Grid
    {
        private static Regex REX_NUMBERS      = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
        private static Regex REX_NUMBERS_STAR = new Regex(@"^[0-9]+\*$", RegexOptions.Compiled);

        public static readonly DependencyProperty FixColumnDefinitionsProperty = DependencyProperty.Register(
            "FixColumnDefinitions",
            typeof(string),
            typeof(FixGrid),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => ((FixGrid)d).OnFixColumnDefinitionsPropertyChanged(e)));

        public string FixColumnDefinitions
        {
            get { return (string)GetValue(FixColumnDefinitionsProperty); }
            set { SetValue(FixColumnDefinitionsProperty, value); }
        }

        public static readonly DependencyProperty FixRowDefinitionsProperty = DependencyProperty.Register(
            "FixRowDefinitions",
            typeof(string),
            typeof(FixGrid),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => ((FixGrid)d).OnFixRowDefinitionsPropertyChanged(e)));

        public string FixRowDefinitions
        {
            get { return (string)GetValue(FixRowDefinitionsProperty); }
            set { SetValue(FixRowDefinitionsProperty, value); }
        }

        private void OnFixColumnDefinitionsPropertyChanged(object e)
        {
            this.ColumnDefinitions.Clear();

            foreach (var token in Tokenize(FixColumnDefinitions))
            {
                if (token.StartsWith("(") && token.EndsWith(")"))
                {
                    var def = new ColumnDefinition();
                    foreach (var subtoken in token[1..^1].Split(","))
                    {
                        if (string.IsNullOrWhiteSpace(subtoken)) continue;

                        if (subtoken.Contains("="))
                        {
                            var split = subtoken.Split("=");
                            var key = split[0].ToLower().Trim();
                            var val = split[1].ToLower().Trim();

                            switch (key)
                            {
                                case "width":
                                case "len":
                                    def.Width = ParseGridLength(val);
                                    break;

                                case "minwidth":
                                case "min":
                                    def.MinWidth = int.Parse(val);
                                    break;

                                case "maxwidth":
                                case "max":
                                    def.MaxWidth = int.Parse(val);
                                    break;

                                case "sharedsizegroup":
                                case "ssg":
                                    def.SharedSizeGroup = val;
                                    break;

                                default:
                                    throw new Exception("Invalid property: " + key);
                            }
                        }
                        else
                        {
                            def.Width = ParseGridLength(subtoken);
                        }
                    }
                    this.ColumnDefinitions.Add(def);
                }
                else
                {
                    this.ColumnDefinitions.Add(new ColumnDefinition { Width = ParseGridLength(token) });
                }
            }
        }

        private void OnFixRowDefinitionsPropertyChanged(object e)
        {
            this.RowDefinitions.Clear();

            foreach (var token in Tokenize(FixRowDefinitions))
            {
                if (token.StartsWith("(") && token.EndsWith(")"))
                {
                    var def = new RowDefinition();
                    foreach (var subtoken in token[1..^1].Split(","))
                    {
                        if (string.IsNullOrWhiteSpace(subtoken)) continue;

                        if (subtoken.Contains("="))
                        {
                            var split = subtoken.Split("=");
                            var key = split[0].ToLower().Trim();
                            var val = split[1].ToLower().Trim();

                            switch (key)
                            {
                                case "height":
                                case "len":
                                    def.Height = ParseGridLength(val);
                                    break;

                                case "minheight":
                                case "min":
                                    def.MinHeight = int.Parse(val);
                                    break;

                                case "maxheight":
                                case "max":
                                    def.MaxHeight = int.Parse(val);
                                    break;

                                case "sharedsizegroup":
                                case "ssg":
                                    def.SharedSizeGroup = val;
                                    break;

                                default:
                                    throw new Exception("Invalid property: " + key);
                            }
                        }
                        else
                        {
                            def.Height = ParseGridLength(subtoken);
                        }
                    }
                    this.RowDefinitions.Add(def);
                }
                else
                {
                    this.RowDefinitions.Add(new RowDefinition { Height = ParseGridLength(token) });
                }
            }
        }

        private static GridLength ParseGridLength(string gl)
        {
            if (gl == "*") return new GridLength(1, GridUnitType.Star);

            if (REX_NUMBERS_STAR.IsMatch(gl)) return new GridLength(int.Parse(gl[0..^1]), GridUnitType.Star);

            if (gl.ToLower() == "auto") return GridLength.Auto;

            if (REX_NUMBERS.IsMatch(gl)) return new GridLength(int.Parse(gl), GridUnitType.Pixel);

            throw new Exception("Invalid GridLength:" + gl);
        }

        private IEnumerable<string> Tokenize(string t)
        {
            var sb = new StringBuilder(t.Length);

            int depth = 0;
            foreach (var chr in t)
            {
                if (chr == '(')
                {
                    if (depth != 0) throw new Exception("Invalid parenthesis depth");

                    if (sb.Length > 0) { yield return sb.ToString(); sb.Clear(); }
                    sb.Append(chr);
                    depth++;
                }
                else if (chr == ')')
                {
                    if (depth != 1) throw new Exception("Invalid parenthesis depth");

                    sb.Append(chr);
                    depth--;
                    yield return sb.ToString();
                    sb.Clear();
                }
                else if (chr == ' ')
                {
                    if (depth == 0)
                    {
                        if (sb.Length>0) { yield return sb.ToString(); sb.Clear(); }
                    }
                    else
                    {
                        sb.Append(chr);
                    }
                }
                else
                {
                    sb.Append(chr);
                }
            }

            if (depth != 0) throw new Exception("Unclosed parenthesis");

            if (sb.Length > 0) { yield return sb.ToString(); sb.Clear(); }
        }
    }
}
