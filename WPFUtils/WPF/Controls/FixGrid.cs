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
     * or FixRowDefinitions="2*|my_group"
     * or FixRowDefinitions="273|my_group"
     * 
     * (same for FixColumnDefinitions, with /s/Height/Width/g )
     * 
     **/
    public class FixGrid: Grid
    {
        private static Regex REX_NUMBERS         = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
        private static Regex REX_NUMBERS_STAR    = new Regex(@"^[0-9]+\*$", RegexOptions.Compiled);
        private static Regex REX_GRIDLEN_GROUPED = new Regex(@"^(?:(?:(?:[0-9]+)?\*)|(auto)|([0-9]+))\|[a-z0-9_\-]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

        private void OnFixColumnDefinitionsPropertyChanged(object _)
        {
            this.ColumnDefinitions.Clear();

            foreach (var token in Tokenize(FixColumnDefinitions))
            {
                this.ColumnDefinitions.Add(ParseDefinition(token, AnyDefinitionType.Column).ToColumnDefinition());
            }
        }

        private void OnFixRowDefinitionsPropertyChanged(object _)
        {
            this.RowDefinitions.Clear();

            foreach (var token in Tokenize(FixRowDefinitions))
            {
                this.RowDefinitions.Add(ParseDefinition(token, AnyDefinitionType.Row).ToRowDefinition());
            }
        }

        private static AnyDefinition ParseDefinition(string token, AnyDefinitionType adt)
        {
            if (token.StartsWith("(") && token.EndsWith(")"))
            {
                var def = new AnyDefinition();
                foreach (var subtoken in token.Substring(1, token.Length - 2).Split(','))
                {
                    if (string.IsNullOrWhiteSpace(subtoken)) continue;

                    if (subtoken.Contains("="))
                    {
                        var split = subtoken.Split('=');
                        var key = split[0].ToLower().Trim();
                        var val = split[1].ToLower().Trim();

                        if (key == "len" || (adt==AnyDefinitionType.Column && key == "width") || (adt == AnyDefinitionType.Row && key == "height"))
                        {
                            def.Value = ParseGridLength(val);
                        }
                        else if (key == "min" || (adt == AnyDefinitionType.Column && key == "minwidth") || (adt == AnyDefinitionType.Row && key == "minheight"))
                        {
                            def.Min = int.Parse(val);
                        }
                        else if (key == "max" || (adt == AnyDefinitionType.Column && key == "maxwidth") || (adt == AnyDefinitionType.Row && key == "maxheight"))
                        {
                            def.Max = int.Parse(val);
                        }
                        else if (key == "ssg" || key == "sharedsizegroup")
                        {
                            def.SharedSizeGroup = val;
                        }
                        else
                        {
                            throw new Exception("Invalid property: " + key);
                        }
                    }
                    else
                    {
                        def.Value = ParseGridLength(subtoken);
                    }
                }
                return def;
            }
            else if (REX_GRIDLEN_GROUPED.IsMatch(token))
            {
                var split = token.Split('|');
                if (split.Length != 2) throw new Exception("Invalid annotated len: " + token);

                return new AnyDefinition { Value = ParseGridLength(split[0]), SharedSizeGroup = split[1] };

            }
            else
            {
                return new AnyDefinition { Value = ParseGridLength(token) };
            }
        }

        private static GridLength ParseGridLength(string gl)
        {
            if (gl == "*") return new GridLength(1, GridUnitType.Star);

            if (REX_NUMBERS_STAR.IsMatch(gl)) return new GridLength(int.Parse(gl.Substring(1, gl.Length - 2)), GridUnitType.Star);

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

internal enum AnyDefinitionType
{
    Column,
    Row,
}

internal class AnyDefinition
{
    public GridLength Value = new GridLength(1, GridUnitType.Star);
    public double Min = 0;
    public double Max = float.PositiveInfinity;
    public string SharedSizeGroup = null;

    public RowDefinition ToRowDefinition() => new RowDefinition
    {
        Height = Value,
        MinHeight = Min,
        MaxHeight = Max,
        SharedSizeGroup = SharedSizeGroup,
    };

    public ColumnDefinition ToColumnDefinition() => new ColumnDefinition
    {
        Width = Value,
        MinWidth = Min,
        MaxWidth = Max,
        SharedSizeGroup = SharedSizeGroup,
    };
}