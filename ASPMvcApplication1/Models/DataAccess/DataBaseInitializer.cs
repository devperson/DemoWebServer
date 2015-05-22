using ASPMvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HServer.Models.DataAccess
{
    /// <summary>
    /// This class used before to make connection to DataBase, 
    /// here we can make configurations such as CreateDatabaseIfNotExists, DropCreateDatabaseAlways, DropCreateDatabaseIfModelChanges
    /// </summary>
    public class DataBaseInitializer : CreateDatabaseIfNotExists<DataBaseContext>
    {

        /// <summary>
        /// Populates database tables with default data if any, note this method will only run when database is being created.
        /// </summary>
        protected override void Seed(DataBaseContext context)
        {
            Driver driver = new Driver();
            driver.FirstName = "Driver1";
            driver.LastName = "D1";
            context.Drivers.Add(driver);


            Menu menu1 = new Menu();
            menu1.Name = "CHICKEN AND CHEESE ENCHILADAS";
            menu1.Price = 8;
            menu1.Description = "homemade chicken & cheese enchiladas with salsa roja, spanish rice, pinto beans & corn (~750 cal)";
            menu1.Image = "img1.jpg";
            //menu1.AvailableDate = DateTime.Now;
            context.Menus.Add(menu1);

            var menu2 = new Menu();
            menu2.Name = "THAI STYLE PORK RICE BOWL";
            menu2.Price = 10;
            menu2.Description = "spicy minced pork with chilies, mint, lime, bell peppers, steamed jasmine rice & sauteed green beans (~425 cal)";
            menu2.Image = "img2.jpg";
            //menu2.AvailableDate = DateTime.Now;
            context.Menus.Add(menu2);

            var menu3 = new Menu();
            menu3.Name = "FOUR CHEESE RAVIOLI WITH WILD MUSHROOM SAUCE";
            menu3.Price = 7;
            menu3.Description = "four cheese ravioli with wild mushroom sauce, asparagus, peas, zucchini, sun dried tomatoes & fontina cheese (~700 cal)";
            menu3.Image = "img3.jpg";
            //menu3.AvailableDate = DateTime.Now;
            context.Menus.Add(menu3);

            var menu4 = new Menu();
            menu4.Name = "MESQUITE GRILLED TRI-TIP STEAK SALAD";
            menu4.Price = 9;
            menu4.Description = "mesquite grilled tri-tip steak salad with romaine, mixed greens, cherry tomatoes, cucumber, red onions & blue cheese dressing (~496 cal) contains (dairy, egg yolk, soybean oil)";
            menu4.Image = "img4.jpg";
            //menu4.AvailableDate = DateTime.Now;
            context.Menus.Add(menu4);


            var menu5 = new Menu();
            menu5.Name = "GARDEN BLISS PHYTONUTRIENT SMOOTHIE";
            menu5.Price = 9;
            menu5.Description = "organic baby spinach, organic celery, organic cucumber, lemon, kiwi, banana, organic green apples, and almond butter. by livblends (~85 cal)";
            menu5.Image = "img5.jpg";
            //menu5.AvailableDate = DateTime.Now.AddDays(1);
            context.Menus.Add(menu5);

            var menu6 = new Menu();
            menu6.Name = "AMARETTO CHEESECAKE";
            menu6.Price = 4;
            menu6.Description = "a dash of amaretto liqueur adds a punch to a classic favorite. a chocolate crust and white and dark chocolate shavings complete this study in dessert perfection (400 calories per serving, contains liquor)";
            menu6.Image = "img6.jpg";
            //menu6.AvailableDate = DateTime.Now.AddDays(1);
            context.Menus.Add(menu6);

            DriverInventory dInventory = new DriverInventory();
            dInventory.Menu = menu1;
            dInventory.Driver = driver;
            dInventory.Count = 5;
            context.DriverInventories.Add(dInventory);

            dInventory = new DriverInventory();
            dInventory.Menu = menu2;
            dInventory.Driver = driver;
            dInventory.Count = 6;
            context.DriverInventories.Add(dInventory);

            dInventory = new DriverInventory();
            dInventory.Menu = menu3;
            dInventory.Driver = driver;
            dInventory.Count = 3;
            context.DriverInventories.Add(dInventory);


            context.SaveChanges();
        }

       
    }
}