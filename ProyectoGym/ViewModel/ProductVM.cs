using ProyectoGym.Model;
using System.Collections.ObjectModel;

public class ProductVM
{
    public ObservableCollection<Product> Products { get; set; }

    public ProductVM()
    {
        Products = new ObservableCollection<Product>
        {
            new Product { Name = "Proteína Whey", ImagePath="/Images/protein.png", Price = 120000 },
            new Product { Name = "Energizante", ImagePath="/Images/energy_drink.png", Price = 30000 },
            new Product { Name = "Mancuernas 10kg", ImagePath="/Images/dumbbell.png", Price = 150000 },
            new Product { Name = "Cuerda para Saltar", ImagePath="/Images/rope.png", Price = 20000 },
            new Product { Name = "Colchoneta", ImagePath="/Images/mat.png", Price = 80000 },
            new Product { Name = "BCAA 500g", ImagePath="/Images/bcaa.png", Price = 100000 }
        };
    }
}