using BirdLab.Data;
using BirdLab.Views;
using BirdLab.Models;
using BirdLab.Services;
using BirdLab.Controllers;
using BirdLab.Repositories;
using BirdLab.Themes;
using Microsoft.EntityFrameworkCore;

namespace BirdLab
{
    static class Program
    {
        const int Width_ = 800, Height_ = 600;

        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("Starting application...");
                var birdService = InitializeServices();
                Console.WriteLine("Services initialized successfully");

                var (birdListView, birdInfoView, birdMenuView) = InitializeViews();
                Console.WriteLine("Views initialized successfully");

                var birdController = new BirdController(birdListView, birdInfoView, birdMenuView, birdService);
                Console.WriteLine("Controller initialized successfully");

                var mainForm = SetupMainForm(birdListView, birdInfoView, birdMenuView);
                Console.WriteLine("Main form setup completed");

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during application startup: {ex}");
                MessageBox.Show($"Error during application startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static BirdService InitializeServices()
        {
            try
            {
                Console.WriteLine("Initializing database connection...");
                var optionsBuilder = new DbContextOptionsBuilder<BirdDbContext>();
                var connectionString = "Server=localhost;Port=3306;Database=birdlabdb;User=root;Password=mysql;";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                var context = new BirdDbContext(optionsBuilder.Options);
                
                Console.WriteLine("Testing database connection...");
                
                // Delete and recreate database (for development only)
                Console.WriteLine("Recreating database...");
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                Console.WriteLine("Database created successfully");
                
                var birdRepository = new EfBirdRepository(context);
                return new BirdService(birdRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing services: {ex}");
                throw;
            }
        }

        private static (BirdListView, BirdInfoView, BirdMenuView) InitializeViews()
        {
            try
            {
                return (
                    new BirdListView(),
                    new BirdInfoView(),
                    new BirdMenuView()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing views: {ex}");
                throw;
            }
        }

        private static Form SetupMainForm(BirdListView birdListView, BirdInfoView birdInfoView, BirdMenuView birdMenuView)
        {
            try
            {
                var mainForm = new Form
                {
                    Text = "Bird Catalog",
                    Size = new Size(Width_, Height_)
                };
                mainForm.BackColor = CatppuccinMochaTheme.Crust;

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up main form: {ex}");
                throw;
            }
        }
    }
}
