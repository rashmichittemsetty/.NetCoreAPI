using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [Route("api/Register/SignupUser")]
        [HttpPost]
        public async Task<JsonResult> SignupUser([FromBody]string info)
        {
            Random random = new Random();
            String number = random.Next(0, 999999).ToString("D6");
            UserInfo signUpModel = JsonConvert.DeserializeObject<UserInfo>(info);
            string emailMsg = "Dear " + signUpModel.Email + ", <br /><br /> '"+ number + "' is the password to Register  <br /><br /> Thanks & Regards, <br />umesh";
            string emailSubject =" Registration OTP";
            // CryptoEngine.Encrypt(signUpModel.Password)
            //DB connection and checking Email once Email is not avaialble in DB we can isert record into DB
            //Usp_CreateUser stored procedure.
            //
            //Insert into dbo.UserInfo (USerEmail,Password,Passcode) values (singupmodel.  etc)
            // Sending Email.  
            //DataTable datatable = AppConverters.CreateDataTable(userModel);
            //SqlCommand cmd = new SqlCommand("[Dic].[Usp_CreateUser]");
            //cmd.Parameters.AddWithValue("@UTT_UserAttribute", datatable).SqlDbType = SqlDbType.Structured;
            //cmd.Parameters.AddWithValue("@UserId", 0).Direction = ParameterDirection.Output;
            //ExecuteInsertStoredProc(cmd);
            await this.SendEmailAsync(signUpModel.Email, emailMsg, emailSubject);
            return new JsonResult(signUpModel);
        }
        [Route("api/Register/ValidateOTP")]
        [HttpPost]
        public JsonResult ValidateOTP([FromBody]string info)
        {
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
            //Sp_ValidateOTP
            //Select statememet to get USer and we need to validate it with otp
            return new JsonResult("Succsessfull");
        }
        //protected int ExecuteInsertStoredProc(SqlCommand command)
        //{
        //    int rowsEffected = 0;
        //    using (var _connection = new SqlConnection(connectionString))
        //    {
        //        command.Connection = _connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        _connection.Open();


        //        try
        //        {
        //            rowsEffected = command.ExecuteNonQuery();
        //        }
        //        finally
        //        {

        //            _connection.Close();
        //        }
        //    }
        //    return rowsEffected;
        //}

        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            try
            {
                // Initialization.  
                var body = msg;
                var message = new MailMessage();

                // Settings.  
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("");
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : "";
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    smtp.UseDefaultCredentials = false;
                    var credential = new NetworkCredential
                    {
                        UserName = "",
                        Password = ""
                    };

                    // Settings.  
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    // Sending  
                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return isSend;
        }

    }


}