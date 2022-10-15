using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace CommandDesignPatternExample1
//{
//public class Document
//{
//    public void Open()
//    {
//        Console.WriteLine("Document Opened");
//    }

//    public void Save()
//    {
//        Console.WriteLine("Document Saved");
//    }

//    public void Close()
//    {
//        Console.WriteLine("Document Closed");
//    }
//}

//public interface ICommand
//{
//    void Execute();
//}

//public class OpenCommand : ICommand
//{
//    private Document document;
//    public OpenCommand(Document doc)
//    {
//        document = doc;
//    }
//    public void Execute()
//    {
//        document.Open();
//    }
//}

//class SaveCommand : ICommand
//{
//    private Document document;
//    public SaveCommand(Document doc)
//    {
//        document = doc;
//    }
//    public void Execute()
//    {
//        document.Save();
//    }
//}

//class CloseCommand : ICommand
//{
//    private Document document;
//    public CloseCommand(Document doc)
//    {
//        document = doc;
//    }
//    public void Execute()
//    {
//        document.Close();
//    }
//}

//public class MenuOptions
//{
//    private ICommand openCommand;
//    private ICommand saveCommand;
//    private ICommand closeCommand;
//    public MenuOptions(ICommand open, ICommand save, ICommand close)
//    {
//        this.openCommand = open;
//        this.saveCommand = save;
//        this.closeCommand = close;
//    }
//    public void clickOpen()
//    {
//        openCommand.Execute();
//    }
//    public void clickSave()
//    {
//        saveCommand.Execute();
//    }
//    public void clickClose()
//    {
//        closeCommand.Execute();
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        Document document = new Document();

//        ICommand openCommand = new OpenCommand(document);
//        ICommand saveCommand = new SaveCommand(document);
//        ICommand closeCommand = new CloseCommand(document);

//        MenuOptions menu = new MenuOptions(openCommand, saveCommand, closeCommand);

//        menu.clickOpen();
//        menu.clickSave();
//        menu.clickClose();

//        Console.ReadKey();
//    }
//}
//}

namespace CommandDesignPatternExample2
{
    public class Tag
    {
        public string TagName { get; set; }
    }

    public class MenuItem
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public int TableNumber { get; set; }
        public List<Tag> Tags { get; set; }

        public void DisplayOrder()
        {
            Console.WriteLine("Table No: " + TableNumber);
            Console.WriteLine("Item: " + Item);
            Console.WriteLine("Quantity: " + Quantity);
            Console.Write("\tTags: ");
            foreach (var item in Tags)
            {
                Console.Write(item.TagName);
            }
        }
    }

    public abstract class OrderCommand
    {
        public abstract void Execute(List order, MenuItem newItem);
    }
    public class NewOrderCommand : OrderCommand
    {
        public override void Execute(List order, MenuItem newItem)
        {
            order.Add(newItem);
        }
    }

    public class RemoveOrderCommand : OrderCommand
    {
        public override void Execute(List order, MenuItem newItem)
        {
            order.Remove(order.Where(x => x.Item == newItem.Item).First());
        }
    }

    public class ModifyOrderCommand : OrderCommand
    {
        public override void Execute(List order, MenuItem newItem)
        {
            var item = order.Where(x => x.Item == newItem.Item).First();
            item.Quantity = newItem.Quantity;
            item.Tags = newItem.Tags;
            item.TableNumber = newItem.TableNumber;
        }
    }

    public class DineChefRestaurant
    {
        public List Orders { get; set; }

        public DineChefRestaurant()
        {
            Orders = new List();
        }

        public void ExecuteCommand(OrderCommand command, MenuItem item)
        {
            command.Execute(this.Orders, item);
        }

        public void ShowOrders()
        {
            foreach (var item in Orders)
            {
                item.DisplayOrder();
            }
        }
    }

    public class DineChef
    {
        private DineChefRestaurant order;
        private OrderCommand orderCommand;
        private MenuItem menuItem;

        public DineChef()
        {
            order = new DineChefRestaurant();
        }

        public void SetOrderCommand(int dineCommand)
        {
            orderCommand = new DineTableCommand().GetDineCommand(dineCommand);
        }

        public void SetMenuItem(MenuItem item)
        {
            menuItem = item;
        }

        public void ExecuteCommand()
        {
            order.ExecuteCommand(orderCommand, menuItem);
        }

        public void ShowCurrentOrder()
        {
            order.ShowOrders();
        }
    }

    public class DineTableCommand
    {
        //Dine table method
        public OrderCommand GetDineCommand(int dineCommand)
        {
            switch (dineCommand)
            {
                case 1:
                    return new NewOrderCommand();
                case 2:
                    return new ModifyOrderCommand();
                case 3:
                    return new RemoveOrderCommand();
                default:
                    return new NewOrderCommand();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DineChef dineChef = new DineChef();
            dineChef.SetOrderCommand(1); /* Insert Order */
            dineChef.SetMenuItem(new MenuItem()
            {
                TableNumber = 1,
                Item = "Super Mega Burger",
                Quantity = 1,
                Tags = new List() { new Tag() { TagName = "Jalapenos," }, new Tag() { TagName = " Cheese," }, new Tag() { TagName = " Tomato" } }
            });
            dineChef.ExecuteCommand();

            dineChef.SetOrderCommand(1); /* Insert Order */
            dineChef.SetMenuItem(new MenuItem()
            {
                TableNumber = 1,
                Item = "Cheese Sandwich",
                Quantity = 1,
                Tags = new List() { new Tag() { TagName = "Spicy Mayo," } }
            });
            dineChef.ExecuteCommand();
            dineChef.ShowCurrentOrder();

            dineChef.SetOrderCommand(3); /* Remove the Cheese Sandwich */
            dineChef.SetMenuItem(new MenuItem()
            {
                TableNumber = 1,
                Item = "Cheese Sandwich"
            });
            dineChef.ExecuteCommand();
            dineChef.ShowCurrentOrder();

            dineChef.SetOrderCommand(2);/* Modify Order */
            dineChef.SetMenuItem(new MenuItem()
            {
                TableNumber = 1,
                Item = "Super Mega Burger",
                Quantity = 1,
                Tags = new List() { new Tag() { TagName = "Jalapenos," }, new Tag() { TagName = " Cheese" } }
            });
            dineChef.ExecuteCommand();
            dineChef.ShowCurrentOrder();
            Console.ReadKey();
        }
    }
}