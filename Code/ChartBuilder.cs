using Wryte.Components;

namespace Wryte.Code
{
    public class ChartBuilder
    {

        // Fields

        private List<Dataset> _datasets;

        private Chart.ChartOptions _options;

        private ChartType _type;

        private List<string> _labels;

        // Constructors 

        public ChartBuilder()
        {
            _datasets = new List<Dataset>();

            _labels = new List<string>();

            _options = new Chart.ChartOptions
            {
                Responsive = true,
                IndexAxis = "x"
            };

            _type = ChartType.Bar;
        }

        // Methods

        public ChartBuilder WithLabels(List<string> labels)
        {
            _labels = labels;
            return this;
        }

        public ChartBuilder WithOptions(Chart.ChartOptions options)
        {
            _options = options;
            return this;
        }

        public ChartBuilder WithScales(Scale scales)
        {
            _options.Scales = scales;
            return this;
        }

        public ChartBuilder WithType(ChartType type)
        {
            _type = type;
            return this;
        }

        public ChartBuilder WithDataset(Dataset dataset)
        {
            _datasets.Add(dataset);
            return this;
        }

        public ChartBuilder WithAnimation(Animation animation)
        {
            _options.Animation = animation;
            return this;
        }

        public ChartBuilder WithDatasets(List<Dataset> datasets)
        {
            _datasets.AddRange(datasets);
            return this;
        }

        public Chart.ChartConfig Build()
        {
            return new Chart.ChartConfig
            {
                Type = _type.GetDescription(),
                Data = new Data
                {
                    Labels = _labels.ToArray(),
                    Datasets = _datasets,
                },
                Options = _options,
            };
        }


    }
}
