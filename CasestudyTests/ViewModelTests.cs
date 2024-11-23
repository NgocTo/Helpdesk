using HelpdeskViewModels;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Student_GetByEmailTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca");
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Student_GetByIdTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetById(1004);
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Student_GetAllTest()
        {
            List<EmployeeViewModel> allStudentVms;
            EmployeeViewModel vm = new();
            allStudentVms = await vm.GetAll();
            Assert.True(allStudentVms.Count > 0);
        }
        [Fact]
        public async Task Student_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Title = "Mrs.",
                Firstname = "Ngoc",
                Lastname = "To",
                Email = "n_to239383@fanshaweonline.ca",
                Phoneno = "(777)777-7777",
                DepartmentId = 100
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public async Task Student_UpdateTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca"); // Student just added in Add test
            vm.Email = vm.Email == "n_to239383@fanshaweonline.ca" ? "default@fanshaweonline.ca" : "n_to239383@fanshaweonline.ca";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
            Assert.True(await vm.Update() == 1);
        }
        [Fact]
        public async Task Student_DeleteTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca"); // Student just added
            Assert.True(await vm.Delete() == 1); // 1 student deleted
        }
    }
}
