using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace Wryte.Components
{
    public partial class Chart
    {

        // Injects

        [Inject]
        public IJSRuntime JS { get; set; }

        // Parameters 

        [Parameter]
        public ChartConfig Config { get; set; }

        [Parameter]
        public Action Loaded { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public string Id { get; set; }

        // Events

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JS.InvokeVoidAsync("setup", Id, Config);
            Loaded?.Invoke();
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await JS.InvokeVoidAsync("setup", Id, Config);
        //    Loaded?.Invoke();
        //}

        public sealed class ChartConfig
        {
            public ChartOptions Options { get; set; }

            public string Type { get; set; }

            public Data Data { get; set; }

            public ChartConfig()
            {
                Options = new ChartOptions();

                Data = new Data();
            }
        }

        public sealed class ChartOptions
        {
            public Animation Animation { get; set; }

            public string IndexAxis { get; set; }

            public bool Responsive { get; set; }

            public Scale Scales { get; set; }

            public ChartOptions()
            {
                Animation = new Animation();

                Scales = new Scale();
            }
        }
    }

    public class Animation
    {
        public double Duration { get; set; }

        public string Easing { get; set; }

        public double Delay { get; set; }

        public bool Loop { get; set; }

        public Animation()
        {
            Easing = "easeOutQuart";

            Duration = 1000;
        }
    }

    public class Scale
    {
        public Axis XAxis { get; set; }

        public Axis YAxis { get; set; }

        public Scale()
        {
            XAxis = new Axis();
            YAxis = new Axis();
        }
    }

    public class Axis
    {
        public Ticks Ticks { get; set; }

        public bool Stacked { get; set; }

        public Axis() => Ticks = new Ticks();
    }

    public class Ticks
    {
        public bool BeginAtZero { get; set; }
    }

    public class Data
    {
        public List<Dataset> Datasets { get; set; }

        public string[] Labels { get; set; }

        public Data() => Datasets = new List<Dataset>();
    }

    public sealed class Dataset
    {
        public string[] BackgroundColor { get; set; }

        //public string BackgroundColor { get; set; }

        public string[] Data { get; set; }

        public string Label { get; set; }
    }

    // ============
    // ===== Enums
    // ============

    public enum ChartType
    {
        [Description("pie")]
        Pie,
        [Description("bar")]
        Bar,
        [Description("line")]
        Line,
        [Description("radar")]
        Radar,
        [Description("doughnut")]
        Doughnut,
        [Description("polarArea")]
        Polar,
    }



}
