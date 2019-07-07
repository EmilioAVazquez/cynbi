using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cynbi.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL;
using Microsoft.AspNetCore.Http;
using cynbi.Data;
using Newtonsoft.Json.Linq;


namespace cynbi.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [Route("[controller]")]
    public class TestController: Controller
    {
      [HttpGet]
      public ActionResult Get(){
        return Ok("asa");
      }
    }

    [Route("[controller]")]
    public class UserController: Controller
    {
        IUserRepository userRepo {set; get;}
        public UserController(IUserRepository _userRepo){
            userRepo = _userRepo;
        }

        [HttpGet]
        public ActionResult Get(){
            return Ok(userRepo.GetAll());
      }
    }

    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;

        public GraphQLController(
            IDocumentExecuter executer,
            IDocumentWriter writer,
            ISchema schema)
        {
            _executer = executer;
            _writer = writer;
            _schema = schema;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync( GraphQLQuery query1)
        {
            var inputs = query1.Variables.ToInputs();
            Trace.WriteLine("!!!!!!!!!");
            Trace.WriteLine(inputs.ToString());
            var queryToExecute = query1.Query;
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = queryToExecute;
                _.OperationName = query1.OperationName;
                _.Inputs = inputs;

                // _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                // _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

            }).ConfigureAwait(false);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

            var json = await _writer.WriteToStringAsync(result);

            // var response = request.CreateResponse(httpResult);
            // response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return Ok(json);
        }
    }

    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject  Variables { get; set; }
    }
}
