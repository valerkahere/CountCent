using CountCent.Model;
using CountCent.Services;

namespace CountCent
{
    public partial class AnalysisPage : ContentPage
    {
        private readonly LocalDbService _LocalDbService;

        public AnalysisPage(LocalDbService localDbService)
        {
            InitializeComponent();
            _LocalDbService = localDbService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }

        private async Task LoadData()
        {
            var dataPoints = await _LocalDbService.GetDataPoints();

            if (dataPoints == null || !dataPoints.Any())
            {
                lbl_AllTimeTotal.Text = "$0.00";
                lbl_AvgPerDay.Text = "$0.00";
                return;
            }

            decimal allTimeTotal = dataPoints.Sum(x => x.Amount);
            lbl_AllTimeTotal.Text = allTimeTotal.ToString("C");

            var uniqueDaysCount = dataPoints.Select(x => x.Date.Date).Distinct().Count();
            if (uniqueDaysCount > 0)
            {
                decimal avgPerDay = allTimeTotal / uniqueDaysCount;
                lbl_AvgPerDay.Text = avgPerDay.ToString("C");
            }
            else
            {
                lbl_AvgPerDay.Text = "$0.00";
            }
        }
    }
}
