using CRUDCLASSES;
using CRUDCLASSES.Model;

namespace ADOCRUD
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (IWhatsAppServices whatsAppService = new WhatsAppService(new WhatsAppDbContext()))
            {
                var userData = new UserViewModel
                {
                    UserName = "SmartPenny",
                    Phone = "070-900-111-15",
                    Picture = "Capital.jpg",
                    About = "Let's do this together",
                    Status_ID = "3"
                };

                var createdUserId = await whatsAppService.CreateUser(userData);

                Console.WriteLine(createdUserId);
                //uncomment it out to do update
                /*var updateResult = await whatsAppService.UpdateUser(
                    22,
                    new UserViewModel()
                    {
                        UserName = "Josh",
                        Phone = "09095002365",
                        Picture = "zoba.jpg",
                        About = "Hello Bae",
                        Status_ID = "1"
                    }
                );

                if (updateResult)
                {
                    Console.WriteLine($"Successfully Updated");
                }
                else
                {
                    Console.WriteLine($"Not Successfully Updated");
                }
                //uncomment it out to do delete
                var deleteUser = await whatsAppService.DeleteUser(27);
 
 
 
                 Console.WriteLine(deleteUser ? $"Successfully Deleted" : $"Not Successfully Deleted");*/

                var allUsers = await whatsAppService.GetUsers();
                foreach (var user in allUsers)
                {
                    Console.WriteLine(
                        $"UserName : {user.UserName} \t Phone : {user.Phone} \t Photo : {user.Picture} \t About : {user.About} \t Status_ID : {user.Status_ID}"
                    );
                }
                //uncomment it out to do get a user
                //var user = await whatsAppService.GetUser(1);
                //Console.WriteLine($"Name : {user.UserName} \t Phone : {user.Phone} \t Photo : {user.Picture} \t About : {user.About} \t Status_ID : {user.Status_ID}");
            }
        }
    }
}
