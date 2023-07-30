namespace Assets.Scripts
{
    public static class APIUrls
    {
        public static string GoogleLogin = "https://octobackendapiwindows.azurewebsites.net/api/auth/google-login";
        public static string GetByUsername = "https://octobackendapiwindows.azurewebsites.net/api/user/getbyusername";
        public static string AddEventPage = "https://octobackendapiwindows.azurewebsites.net/api/event/add";
        public static string CreateImmediateMeeting = "https://octobackendapiwindows.azurewebsites.net/api/event/create-instance";
        public static string GetUpcomingList = "https://octobackendapiwindows.azurewebsites.net/api/event/get-upcomings";
        public static string GetToDoByCategory = "https://octobackendapiwindows.azurewebsites.net/api/todo/getbycategory";
        public static string AddToDoByCategory = "https://octobackendapiwindows.azurewebsites.net/api/todo/add-task";
        public static string SetToDoStatus = "https://octobackendapiwindows.azurewebsites.net/api/todo/set-status";
        public static string SetToDoCategoryDeadline = "https://octobackendapiwindows.azurewebsites.net/api/todo/set-deadline";
        public static string ClearToDoCategory = "https://octobackendapiwindows.azurewebsites.net/api/todo/delete-completed";
    }
}