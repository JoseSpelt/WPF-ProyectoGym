using ProyectoGym.Model;
using System.Collections.ObjectModel;

public class MembershipVM
{
    public ObservableCollection<Membership> Memberships { get; set; }

    public MembershipVM()
    {
        
        Memberships = new ObservableCollection<Membership>
        {
            new Membership
            {
                Name = "Básico",
                Description = "Acceso limitado a las instalaciones. Ideal para los que recién comienzan.",
                Price = 500000, 
                ImagePath = "/Images/membresia_basico.png"
            },
            new Membership
            {
                Name = "Medio",
                Description = "Acceso a todas las instalaciones, clases grupales y más.",
                Price = 800000,
                ImagePath = "/Images/membresia_medio.png"
            },
            new Membership
            {
                Name = "Experto",
                Description = "Acceso total, incluye servicios premium como entrenadores personales.",
                Price = 1200000,
                ImagePath = "/Images/membresia_experto.png"
            }
        };
    }
}