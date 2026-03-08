namespace ITI_MVC_Project.Consts
{
    public static class RegexPatterns
    {
        public const string Password = @"(?=.*[0-9])(?=.*[a-z])(?=(.*)).{6,}";
    }
}