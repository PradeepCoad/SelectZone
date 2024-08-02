using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SelectZone.Models;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Text;
using Npgsql;
using System.Collections.ObjectModel;

namespace SelectZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }





        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }





        //Call to save files to host directry and file data into database
        [HttpPost]
        public async Task<IActionResult> FileMaster(List<IFormFile> Files)
        {
            try
            {
                FileMaster fileMasterBEs = new FileMaster();
                StringBuilder savedFileNameWithPath = new StringBuilder();
                string username = "SuperAdmin";

                if (Files != null)
                {
                    if (Files.Count > 0)
                    {

                        string path = Path.Combine("D:\\SelectZone\\Home\\FileMaster\\", username,DateTime.Today.ToString("yyyy-MM-dd"));

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        foreach (var file in Files)
                        {
                            string filenamewithpath = Path.Combine(path, file.FileName);

                            using (var steam = new FileStream(filenamewithpath, FileMode.Create))
                            {
                                file.CopyTo(steam);
                            }
                            savedFileNameWithPath.Append(filenamewithpath);
                            savedFileNameWithPath.Append(',');
                        }

                    }

                }
                

                var result = await FileMasterInsert(savedFileNameWithPath.ToString(), 1);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.ToString());

            }
            return RedirectToAction("Index");
        }


        public async Task<string> FileMasterInsert(string saveFilePaths, int ProcessType)
        {
            string strReturnValue = "";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=admin;Database=SelectZone"))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("public.usp_insert_filemaster", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_outputparam", NpgsqlTypes.NpgsqlDbType.Integer).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("p_docpath", saveFilePaths == null ? DBNull.Value : saveFilePaths);

                        cmd.Parameters.AddWithValue("processtype", ProcessType);

                        await cmd.ExecuteNonQueryAsync();

                        strReturnValue = cmd.Parameters["p_outputparam"].Value.ToString();
                        connection.Close();
                    }
                }

            }
            catch
            {
            }
            return strReturnValue;
        }



        //Call to get all file data as per date requested form client side
        [HttpGet]
        public async Task<ObservableCollection<FileMaster>> getFilesData(DateTime date)
        {
            DataTable dt = new DataTable();
            ObservableCollection<FileMaster> allFiles = new ObservableCollection<FileMaster>();
            try
            {
                using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=admin;Database=SelectZone"))
                {
                    conn.Open();
                    //await conn.OpenAsync();

                    // Create a command to execute the SQL
                    using (var cmd = new NpgsqlCommand("SELECT * from public.retreivefiles(@date,@processtype)", conn))
                    {
                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@processtype", 1);

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            // Fill the DataTable with the result of the function
                            adapter.Fill(dt);
                            allFiles = await ConvertDataTableToCollection(dt);
                        }
                    }
                }
            }
            catch { }
            return allFiles;
        }

        private async Task<ObservableCollection<FileMaster>> ConvertDataTableToCollection(DataTable dt)
        {
            ObservableCollection<FileMaster> currentFile = new ObservableCollection<FileMaster>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FileMaster fileMaster = new FileMaster();


                    if (dt.Rows[i]["amdid"] != DBNull.Value)
                        fileMaster.amdID = Convert.ToInt32(dt.Rows[i]["amdid"].ToString());

                    if (dt.Rows[i]["Docpath"] != DBNull.Value)
                        fileMaster.docPath = dt.Rows[i]["Docpath"].ToString();



                    if (System.IO.File.Exists(fileMaster.docPath))
                    {
                        // Read the file into a byte array
                        byte[] image = System.IO.File.ReadAllBytes(fileMaster.docPath);
                        fileMaster.data.Add(image);

                    }

                    currentFile.Add(fileMaster);
                }
            }
            catch
            {
            }
            return await Task.FromResult(currentFile);
        }


        //It's call for removing files from particular date
        public async Task<IActionResult> FileStatusUpdate(int amdid)
        {
            string result = "";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=admin;Database=SelectZone"))
                {
                    connection.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("public.usp_update_file_status", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("outputparam", NpgsqlTypes.NpgsqlDbType.Varchar).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("p_amdid", amdid);
                        cmd.Parameters.AddWithValue("processtype", 1);

                        await cmd.ExecuteNonQueryAsync();

                        result = cmd.Parameters["outputparam"].Value.ToString();
                        connection.Close();
                    }

                }

            }
            catch
            {
            }
            return Json(result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
