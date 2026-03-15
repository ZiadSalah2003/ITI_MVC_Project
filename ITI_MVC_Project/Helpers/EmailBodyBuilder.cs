namespace ITI_MVC_Project.Helpers
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template, Dictionary<string, string> placeholders)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{template}.html");
            var body = File.ReadAllText(templatePath);

            foreach (var item in placeholders)
                body = body.Replace(item.Key, item.Value);

            return body;
        }
    }
}
