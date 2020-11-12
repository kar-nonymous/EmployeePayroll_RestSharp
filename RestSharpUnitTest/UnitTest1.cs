using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RestSharpUnitTest
{
    /// <summary>
    /// Creating the class to take the different entries of an employee
    /// </summary>
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Calling RestClient class
        /// </summary>
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }
        /// <summary>
        /// UC 1:
        /// Setting up the method to get all the employees.
        /// </summary>
        /// <returns></returns>
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ShouldReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(3, dataResponse.Count);
            foreach(Employee employee in dataResponse)
            {
                System.Console.WriteLine("ID: " + employee.id + " Name: " + employee.name + " Salary: " + employee.salary);
            }
        }
        /// <summary>
        /// UC 2:
        /// POST api will add employee provided to the json file created
        /// </summary>
        [TestMethod]
        public void OnCallingPOSTApi_ShouldAddEmployee()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Clark");
            jObjectBody.Add("salary", "15000");

            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC 3:
        /// POST api will add multiple employees to the json file created
        /// </summary>
        [TestMethod]
        public void OnCallingPOSTApi_ShouldAddMultipleEmployee()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee { name = "Peter", salary = "2000" });
            employees.Add(new Employee { name = "Jobs", salary = "20000" });
            employees.ForEach(employee =>
            {
                //Arrange
                RestRequest request = new RestRequest("/employees", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("name", employee.name);
                jObjectBody.Add("salary", employee.salary);

                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name, dataResponse.name);
                Assert.AreEqual(employee.salary, dataResponse.salary);
                System.Console.WriteLine(response.Content);
            });
        }
        /// <summary>
        /// UC 4:
        /// PUT api will update emplyee name and salary in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingPUTApi_ShouldUpdateEmployee()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees/1", Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Rahul");
            jObjectBody.Add("salary", "50000");

            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Rahul", dataResponse.name);
            Assert.AreEqual("50000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
    }
}
