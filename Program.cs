using BirdLab.Data;
using BirdLab.Views;
using BirdLab.Models;
using BirdLab.Services;
using BirdLab.Controllers;
using BirdLab.Repositories;

namespace BirdLab
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var birdService = InitializeServices();
            var (birdListView, birdInfoView, birdMenuView) = InitializeViews();

            var birdController = new BirdController(birdListView, birdInfoView, birdMenuView, birdService);

            Application.Run(SetupMainForm(birdListView, birdInfoView, birdMenuView));
        }

        private static BirdService InitializeServices()
        {
            var jsonStorage = new JsonDataStorage<BirdDTO>("birds.json");
            var birdRepository = new BirdRepository(jsonStorage);
            return new BirdService(birdRepository);
        }

        private static (BirdListView, BirdInfoView, BirdMenuView) InitializeViews()
        {
            return (
                new BirdListView(),
                new BirdInfoView(),
                new BirdMenuView()
            );
        }

        private static Form SetupMainForm(BirdListView birdListView, BirdInfoView birdInfoView, BirdMenuView birdMenuView)
        {
            var mainForm = new Form
            {
                Text = "Bird Catalog",
                Size = new Size(800, 600)
            };

            var layout = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };

            layout.Panel1.Controls.Add(birdListView);

            var panel2 = new Panel { Dock = DockStyle.Fill };
            var flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            flowLayoutPanel.Controls.Add(birdInfoView);
            flowLayoutPanel.Controls.Add(birdMenuView);

            panel2.Controls.Add(flowLayoutPanel);
            layout.Panel2.Controls.Add(panel2);

            mainForm.Controls.Add(layout);

            return mainForm;
        }
    }
}
