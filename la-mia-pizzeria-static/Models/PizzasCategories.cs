namespace la_mia_pizzeria_crud_mvc.Models
{
    public class PizzasCategories
    {
        public Pizza Pizza { get; set; }
        public List <Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<int> SelectedTags { get; set; }
        public PizzasCategories()
        {
            Pizza = new Pizza();
            Categories = new List<Category>();
            Tags = new List<Tag>();
            SelectedTags = new List<int>();
        }

    }
}
