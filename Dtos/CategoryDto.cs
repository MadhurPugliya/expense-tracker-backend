namespace ExpenseTracker.Dtos
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // public string Icon { get; set; }
    }

    public class CategoryUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // public string Icon { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
}
