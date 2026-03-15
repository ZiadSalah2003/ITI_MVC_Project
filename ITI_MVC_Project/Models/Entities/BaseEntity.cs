namespace ITI_MVC_Project.Models.Entities
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
    }
}
