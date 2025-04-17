using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace gui.Tests;

[TestClass]

public class DemoTest : PageTest

{

    private IPlaywright _playwright;

    private IBrowser _browser;

    private IBrowserContext _browserContext;

    private IPage _page;


    [TestInitialize]

    public async Task Setup()

    {

        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions

        {

            Headless = true,

            SlowMo = 500 // Lägger in en fördröjning så vi kan se vad som händer

        });

        _browserContext = await _browser.NewContextAsync();

        _page = await _browserContext.NewPageAsync();

    }


    [TestCleanup]

    public async Task Cleanup()

    {

        await _browserContext.CloseAsync();

        await _browser.CloseAsync();

        _playwright.Dispose();

    }



    [TestMethod]
    public async Task SuperAdminBlockUnblockCompanyTest()

    {
        await WhenImAt("super_gris@mail.com","kung","Companies" );

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Eco EnterPrises")} ).GetByRole(AriaRole.Button, new() {Name="block"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Companies"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Eco EnterPrises")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();

    }

       [TestMethod]
    public async Task SuperAdminCreateDeleteCompanyTest()

    {
         await WhenImAt("super_gris@mail.com","kung","Companies" );
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Add Company"}).ClickAsync();
        
         await _page.GetByLabel("Name").FillAsync("Gui Test Company");
         await _page.GetByLabel("Email").FillAsync("test.company@swinesync.com");
         await _page.GetByLabel("Phone").FillAsync("00000000");
         await _page.GetByLabel("Description").FillAsync("Gui Test Company");
         await _page.GetByLabel("Domain").FillAsync("http://guiTestfake.com");
         await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
         await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();

        var createdCompany= _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Gui Test Company")} ).GetByRole(AriaRole.Button, new() {Name="delete"});
        await Expect(createdCompany).ToBeVisibleAsync();
        await createdCompany.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();


    }

  [TestMethod]
 public async Task SuperAdminEditCompanyTest()

    {
          await WhenImAt("super_gris@mail.com","kung","Companies" );

           await GoToEdit("Tech Solutions");
        await _page.GetByLabel("Domain").FillAsync("http://contact@techsolutions_test.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        
        var changedCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("http://contact@techsolutions_test.com")} ).GetByRole(AriaRole.Button, new() {Name="Edit"});
        await Expect(changedCompany).ToBeVisibleAsync();
        await changedCompany.ClickAsync(); 
        await _page.GetByLabel("Domain").FillAsync("http://contact@techsolutions.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();


    }


[TestMethod]

 public async Task ChatTest()

    {
        await _page.GotoAsync("http://localhost:5000/customer/leak-dispenser-002/chat");
        await _page.GetByRole(AriaRole.Textbox).FillAsync("customer test");
        await _page.GetByRole(AriaRole.Button, new() {Name="Send"}).ClickAsync(); 

        await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync("tryne@hotmail.com"); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync("asd123"); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();

        await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Water dispenser is leaking")}).GetByRole(AriaRole.Link).ClickAsync(); 
        await Expect( _page.Locator("css=.chat-left-message").Nth(-1).GetByText("customer test")).ToHaveTextAsync("customer test");
        await _page.GetByRole(AriaRole.Textbox).FillAsync("agent test");
        await _page.GetByRole(AriaRole.Button, new() {Name="Send"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync(); 

         await _page.GotoAsync("http://localhost:5000/customer/leak-dispenser-002/chat");
         await Expect( _page.Locator("css=.chat-left-message").Nth(-1).GetByText("agent test")).ToHaveTextAsync("agent test");

      



    }


[TestMethod]

 public async Task SuperAdminEditAdminTest(){
   await WhenImAt("super_gris@mail.com","kung","Admins" );
    await GoToEdit("Boss Hog");
    await _page.GetByLabel("Email").FillAsync("boss-hog@company.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        
        var changedAdmin=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("boss-hog@company.com")} ).GetByRole(AriaRole.Button, new() {Name="Edit"});
        await Expect(changedAdmin).ToBeVisibleAsync();
        await changedAdmin.ClickAsync(); 
        await _page.GetByLabel("Email").FillAsync("boss-hog@company2.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();


    }

 
    [TestMethod]
    public async Task SuperAdminBlockUnblockAdminTest()

    {
        await WhenImAt("super_gris@mail.com","kung","Admins" );

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Pelle Svinsson")} ).GetByRole(AriaRole.Button, new() {Name="block"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Admins"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("pelle.svinsson@fakemail.com")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   

    }



       [TestMethod]
    public async Task SuperAdminCreateDeleteAdminTest()

    {
         await WhenImAt("super_gris@mail.com","kung","Admins" );
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Add Admin"}).ClickAsync();
        
         await _page.GetByLabel("Name").FillAsync("Test Admin");
         await _page.GetByLabel("Email").FillAsync("test.admin@swinesync.com");
        await _page.GetByLabel("Company").SelectOptionAsync("Global Ventures");

         await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
         await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
         await Task.Delay(2000); //2000 = 2sec
        await _page.GotoAsync("http://localhost:5000/admins");
        var createdItem= _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Test Admin")} ).GetByRole(AriaRole.Button, new() {Name="delete"});
        await Expect(createdItem).ToBeVisibleAsync();
        await createdItem.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   
    }


/////////////////////////////////////////////////////////////////////////////////////////////////////////
// /  admin tests
// / 

//////////// Products


       [TestMethod]
    public async Task AdminCreateDeleteProductTest()

    {
         await WhenImAt("grune@grymt.se","hejhej","Products" );
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Add product"}).ClickAsync();
        
         await _page.GetByLabel("Name").FillAsync("Test Product");
         await _page.GetByLabel("Description").FillAsync("a test product");
         await _page.GetByLabel("Price").FillAsync("100");
         await _page.GetByLabel("Category").FillAsync(" test category");
 

         await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
         await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();

        var createdItem= _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Test Product")} ).GetByRole(AriaRole.Button, new() {Name="delete"});
        await Expect(createdItem).ToBeVisibleAsync();
        await createdItem.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   
    }


 [TestMethod]
    public async Task AdminBlockUnblockProductTest()
    {    
        await WhenImAt("grune@grymt.se","hejhej","Products" );

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Mud Bath")} ).GetByRole(AriaRole.Button, new() {Name="block"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Products"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Mud Bath")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   

    }


[TestMethod]

 public async Task AdminEditProductTest(){
  await WhenImAt("grune@grymt.se","hejhej","Products" );
    await GoToEdit("Organic Swine Feed");
    await _page.GetByLabel("Category").FillAsync("Test Category");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        
        var changedItem=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Test Category")} ).GetByRole(AriaRole.Button, new() {Name="Edit"});
        await Expect(changedItem).ToBeVisibleAsync();
        await changedItem.ClickAsync(); 
        await _page.GetByLabel("Category").FillAsync("Feed");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();


    }


//////////////////// Support Agents 
// / 
// / 
// / 

       [TestMethod]
    public async Task AdminCreateDeleteSupportAgent()

    {
         await WhenImAt("grune@grymt.se","hejhej","Support Agents" );
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Add Support Agent"}).ClickAsync();
        
         await _page.GetByLabel("Name").FillAsync("Test Agent");
         await _page.GetByLabel("Email").FillAsync("test.agent@swinesync.com");
         await _page.GetByRole(AriaRole.Listitem).Filter(new(){Has=_page.GetByText("Account Management")}).ClickAsync();
        

         await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
         await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        await Task.Delay(3000); //2000 = 2sec
        await _page.GotoAsync("http://localhost:5000/agents");
        var createdItem= _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Test Agent")} ).GetByRole(AriaRole.Button, new() {Name="delete"});
        await Expect(createdItem).ToBeVisibleAsync();
        await createdItem.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   
    }


 [TestMethod]
    public async Task AdminBlockUnblockAgentTest()
    {    
        await WhenImAt("grune@grymt.se","hejhej","Support Agents" );

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Pig Pal")} ).GetByRole(AriaRole.Button, new() {Name="block"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Agents"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Pig Pal")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   

    }


[TestMethod]

 public async Task AdminEditAgentTest(){
  await WhenImAt("grune@grymt.se","hejhej","Support Agents" );
    await GoToEdit("Nasse Puh");
    await _page.GetByLabel("Name").FillAsync("Nalle Puh");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        
        var changedItem=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Nalle Puh")} ).GetByRole(AriaRole.Button, new() {Name="Edit"});
        await Expect(changedItem).ToBeVisibleAsync();
        await changedItem.ClickAsync(); 
        await _page.GetByLabel("Name").FillAsync("Nasse Puh");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();


    }





 [TestMethod]
    public async Task AdminCreateDeleteCategoryTest()

    {
         await WhenImAt("grune@grymt.se","hejhej","Categories" );
        
        await _page.GetByRole(AriaRole.Textbox,new(){Name="Category Name.."}).FillAsync("Test Category");
        await _page.GetByRole(AriaRole.Button,new(){Name="Add Category"}).ClickAsync();
        var createdItem= _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Test Category")} ).GetByRole(AriaRole.Button, new() {Name="Delete"});
        await Expect(createdItem).ToBeVisibleAsync();
        await createdItem.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
    }



 [TestMethod]
    public async Task AdminBlockUnblockCategoryTest()
    {    
         await WhenImAt("grune@grymt.se","hejhej","Categories" );

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Product Feedback")} ).GetByRole(AriaRole.Button, new() {Name="Inactivate"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Categories"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Product Feedback")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
   

    }





public async Task WhenImAt(string email,string password , string buttonName){
            await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync(email); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync(password); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new(){Name=buttonName}).ClickAsync();
}

public async Task GoToEdit(string itemName){
    await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText(itemName)} ).GetByRole(AriaRole.Button, new() {Name="Edit"}).ClickAsync();
}


}