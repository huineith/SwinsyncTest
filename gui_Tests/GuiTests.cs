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

            Headless = false,

            SlowMo = 1000 // Lägger in en fördröjning så vi kan se vad som händer

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
        await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync("super_gris@mail.com"); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync("kung"); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();

       
        await _page.GetByRole(AriaRole.Button, new(){Name="Companies"}).ClickAsync();

         await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Global Ventures")} ).GetByRole(AriaRole.Button, new() {Name="block"}).ClickAsync();
        
        await _page.GetByRole(AriaRole.Button, new(){Name="Show Inactive Companies"}).ClickAsync();

        
        var inactiveCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Global Ventures")} ).GetByRole(AriaRole.Button, new() {Name="Activate"}); 
        
        await Expect(inactiveCompany).ToBeVisibleAsync();
        
        await inactiveCompany.ClickAsync();

        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
        // Kontrollera att knappen "Electronics" finns på sidan

    }

       [TestMethod]
    public async Task SuperAdminCreateDeleteCompanyTest()

    {
        await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync("super_gris@mail.com"); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync("kung"); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new(){Name="Companies"}).ClickAsync();

        
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
        // Kontrollera att knappen "Electronics" finns på sidan

    }

  [TestMethod]
 public async Task SuperAdminEditCompanyTest()

    {
        await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync("super_gris@mail.com"); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync("kung"); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new(){Name="Companies"}).ClickAsync();

        
        await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Tech Solutions")} ).GetByRole(AriaRole.Button, new() {Name="Edit"}).ClickAsync();
        await _page.GetByLabel("Domain").FillAsync("http://contact@techsolutions_test.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="⬅️ Back"}).ClickAsync();
        
        var changedCompany=_page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("http://contact@techsolutions_test.com")} ).GetByRole(AriaRole.Button, new() {Name="Edit"});
        await Expect(changedCompany).ToBeVisibleAsync();
        await changedCompany.ClickAsync(); 
        await _page.GetByLabel("Domain").FillAsync("http://contact@techsolutions.com");
        await _page.GetByRole(AriaRole.Button, new() {Name="Save"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync();
        // Kontrollera att knappen "Electronics" finns på sidan

    }


[TestMethod]

 public async Task SuperAdminEditCompanyTest()

    {
        await _page.GotoAsync("http://localhost:5000/customer/low-pressure-shower-048/chat");
        await _page.GetByRole(AriaRole.Textbox).FillAsync("customer test");
        await _page.GetByRole(AriaRole.Button, new() {Name="Send"}).ClickAsync(); 

        await _page.GotoAsync("http://localhost:5000/");
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Email.."}).FillAsync("tryne@hotmail.com"); 
        await _page.GetByRole(AriaRole.Textbox, new(){Name="Password.."}).FillAsync("asd123"); 
        await _page.GetByRole(AriaRole.Button, new(){Name="login!"}).ClickAsync();

        await _page.GetByRole(AriaRole.Listitem).Filter(new(){ Has= _page.GetByText("Shower system pressure too low")}).GetByRole(AriaRole.Link).ClickAsync(); 
        await Expect( _page.Locator("css=.chat-left-message").Nth(-1).GetByText("customer test")).ToHaveTextAsync("customer test");
        await _page.GetByRole(AriaRole.Textbox).FillAsync("agent test");
        await _page.GetByRole(AriaRole.Button, new() {Name="Send"}).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() {Name="Sign Out"}).ClickAsync(); 

         await _page.GotoAsync("http://localhost:5000/customer/low-pressure-shower-048/chat");
         await Expect( _page.Locator("css=.chat-left-message").Nth(-1).GetByText("agent test")).ToHaveTextAsync("agent test");

      



    }



}