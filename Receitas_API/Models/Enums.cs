namespace Receitas_API.Models
{
    public class Enums
    {
        public enum AlertMessageType
        {
            Error, Warning, Success, Info
        }
        public enum Modules
        {
            Recipes
        }

        public enum TransactionImage
        {
            Create,
            Read,
            Update,
            Delete,
            Backup,
            Zip,
            Warning,
            Info,
        }
    }
}
