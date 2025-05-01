
using System.Data.SqlClient;
using System.Security.Policy;
using System;
using System.Security.Cryptography;

class Program
{
    static string connectionString = "Server=DESKTOP-0LUMDSN;Database=RestaurantManagementSystem;Trusted_Connection=True;";
    static int orderId = 13;
    static void Main()
    {
        while (true)
        {
            var p1 = "Admin123";
            var p2 = "Manager123";
            var p3 = "Staff123";

            Console.Write("Enter user first name: ");
            var fname = Console.ReadLine();
            Console.Write("Enter user last name: ");
            var lname = Console.ReadLine();
            Console.Write("Enter user password: ");
            var password = Console.ReadLine();

            if ((fname == "John") && (lname == "Doe") && (password == p1))
            {
                while (true)
                {
                    Console.WriteLine("Hello " + fname + " " + lname);
                    Console.WriteLine("\n--- Admin Menu ---");
                    Console.WriteLine("1. Add User");
                    Console.WriteLine("2. View Users");
                    Console.WriteLine("3. Update User");
                    Console.WriteLine("4. Delete User");
                    Console.WriteLine("5. Exit");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": AddUser(); break;
                        case "2": ViewUsers(); break;
                        case "3": UpdateUser(); break;
                        case "4": DeleteUser(); break;
                        case "5": return;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
            }
            else if ((fname == "Walter" && lname == "White" && password == p2) || (fname == "Gustavo" && lname == "Fring" && password == p2) || (fname == "Tuco" && lname == "Salamanca" && password == p2))
            {
                while (true)
                {
                    Console.WriteLine("Hello " + fname + " " + lname);
                    Console.WriteLine("\n--- Manager Menu ---");
                    Console.WriteLine("1. Create Reservation");
                    Console.WriteLine("2. View Orders");
                    Console.WriteLine("3. Update Table Status");
                    Console.WriteLine("4. Exit");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": CreateReservation(); break;
                        case "2": ViewOrders(); break;
                        case "3": UpdateTable(); break;
                        case "4": return;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
            }
            else if (password == p3)
            {
                while (true)
                {
                    Console.WriteLine("\n--- Staff Menu ---");
                    Console.WriteLine("1. Take Order");
                    Console.WriteLine("2. Update Table Status");
                    Console.WriteLine("3. Generate Bill");
                    Console.WriteLine("4. Exit");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": TakeOrder(); break;
                        case "2": UpdateTable(); break;
                        case "3": GenerateBill(); break;
                        case "4": return;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Unrecognized name or password...");
            }
            break;
        }
    }
    static void AddUser()
    {
        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Role ID: ");
        var roleid = Console.ReadLine();
        string query = "INSERT INTO Users (firstname, lastname, roleid) VALUES (@firstname, @lastname, @roleid)";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@firstname", firstName);
        cmd.Parameters.AddWithValue("@lastname", lastName);
        cmd.Parameters.AddWithValue("@roleid", roleid);
        conn.Open();
        cmd.ExecuteNonQuery();
        Console.WriteLine("User added successfully.");
    }
    static void ViewUsers()
    {
        string query = "SELECT * FROM Users";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"{reader["userid"]}: {reader["firstname"]} {reader["lastname"]}");
        }
    }
    static void UpdateUser()
    {
        Console.Write("Enter User ID to update: ");
        int userid = int.Parse(Console.ReadLine());
        Console.Write("New First Name: ");
        var firstname = Console.ReadLine();
        Console.Write("New Last Name: ");
        var lastname = Console.ReadLine();
        Console.Write("New Role ID: ");
        var roleid = Console.ReadLine();
        string query = "UPDATE Users SET firstname=@firstname, lastname=@lastname, roleid = @roleid WHERE userid = @userid";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@firstname", firstname);
        cmd.Parameters.AddWithValue("@lastname", lastname);
        cmd.Parameters.AddWithValue("@roleid", roleid);
        cmd.Parameters.AddWithValue("@userid", userid);
        conn.Open();
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "User updated." : "User not found.");
    }
    static void DeleteUser()
    {
        Console.Write("Enter User ID to delete: ");
        int userid = int.Parse(Console.ReadLine());
        string query = "DELETE FROM Users WHERE userid=@userid";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@userid", userid);
        conn.Open();
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "User deleted." : "User not found.");
    }

    static void UpdateTable()
    {
        Console.Write("Enter Table Number to update: ");
        int tableid = int.Parse(Console.ReadLine());
        Console.Write("Enter new table status (Taken/Open): ");
        var status = Console.ReadLine();
        string query = "UPDATE Tables SET status = @status WHERE tableid = @tableid";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@status", status);
        cmd.Parameters.AddWithValue("@tableid", tableid);
        conn.Open();
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Table updated." : "Table not found.");
    }

    static void CreateReservation()
    {
        Console.Write("Enter User ID: ");
        int userId = int.Parse(Console.ReadLine());

        Console.Write("Enter Table ID: ");
        int tableId = int.Parse(Console.ReadLine());

        Console.Write("Enter reservation time (yyyy-MM-dd HH:mm): ");
        string inputTime = Console.ReadLine();

        if (!DateTime.TryParse(inputTime, out DateTime reservationTime))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        string query = "INSERT INTO Reservations (userid, tableid, reservation_time) VALUES (@userid, @tableid, @reservation_time)";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@userid", userId);
        cmd.Parameters.AddWithValue("@tableid", tableId);
        cmd.Parameters.AddWithValue("@reservation_time", reservationTime);

        conn.Open();
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Reservation created." : "Failed to create reservation.");
    }

    static void ViewOrders()
    {
        string query = @"
            SELECT o.orderid, t.tablenumber, u.firstname + ' ' + u.lastname AS CreatedBy,
                   o.order_status, o.order_date
            FROM Orders o
            JOIN Users u ON o.userid = u.userid
            JOIN Tables t ON o.tableid = t.tableid";

        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"Order #{reader["orderid"]} - Table {reader["tablenumber"]} - By {reader["CreatedBy"]} - Status: {reader["order_status"]} - Date: {reader["order_date"]}");
        }
    }
    static void GenerateBill()
    {
        Console.Write("Enter Table ID: ");
        int tableId = int.Parse(Console.ReadLine());
        string query = @"
        SELECT o.orderid, u.firstname + ' ' + u.lastname AS staff, o.order_date
        FROM Orders o
        JOIN Users u ON o.userid = u.userid
        WHERE o.tableid = @tableid AND o.order_status != 'Completed'";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@tableid", tableId);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            int orderId = Convert.ToInt32(reader["orderid"]);
            Console.WriteLine($"\nBill for Table {tableId}");
            Console.WriteLine($"Order ID: {orderId}");
            Console.WriteLine($"Served by: {reader["staff"]}");
            Console.WriteLine($"Order Time: {reader["order_date"]}");
            string detailQuery = @"
            SELECT od.quantity, mi.itemname, od.price, (od.quantity * od.price) AS total
            FROM OrderDetails od
            JOIN MenuItems mi ON od.itemid = mi.itemid
            WHERE od.orderid = @orderid";
            SqlCommand detailCmd = new SqlCommand(detailQuery, conn);
            detailCmd.Parameters.AddWithValue("@orderid", orderId);
            SqlDataReader detailReader = detailCmd.ExecuteReader();
            decimal totalAmount = 0;
            while (detailReader.Read())
            {
                decimal itemTotal = Convert.ToDecimal(detailReader["total"]);
                Console.WriteLine($"{detailReader["itemname"]} x{detailReader["quantity"]} - ₱{itemTotal}");
                totalAmount += itemTotal;
            }
            Console.WriteLine($"\nTotal Bill: ₱{totalAmount}");
            string updateOrderStatus = "UPDATE Orders SET order_status = 'Completed' WHERE orderid = @orderid";
            SqlCommand updateCmd = new SqlCommand(updateOrderStatus, conn);
            updateCmd.Parameters.AddWithValue("@orderid", orderId);
            updateCmd.ExecuteNonQuery();
            Console.WriteLine("Bill generated and order marked as completed.");
        }
        else
        {
            Console.WriteLine("No pending order found for this table.");
        }
    }

    static void TakeOrder()
    {
        Console.Write("Enter User ID: ");
        var staffId = Console.ReadLine();

        Console.Write("Enter Table ID: ");
        int tableId = int.Parse(Console.ReadLine());
        string orderQuery = "INSERT INTO Orders (userid, tableid, order_status) VALUES (@userid, @tableid, 'Pending')";
        int orderId = 13;
        orderId++;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(orderQuery, conn);
            cmd.Parameters.AddWithValue("@userid", staffId);
            cmd.Parameters.AddWithValue("@tableid", tableId);
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT SCOPE_IDENTITY()";
        }
        while (true)
        {
            Console.Write("Enter Item ID (or 0 to finish): ");
            int itemId = int.Parse(Console.ReadLine());
            if (itemId == 0)
                break;
            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());
            decimal price = 0;
            string priceQuery = "SELECT price FROM MenuItems WHERE itemid = @itemid";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(priceQuery, conn);
                cmd.Parameters.AddWithValue("@itemid", itemId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    price = Convert.ToDecimal(result);
            }
            string orderDetailQuery = "INSERT INTO OrderDetails (orderid, itemid, quantity, price) VALUES (@orderid, @itemid, @quantity, @price)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(orderDetailQuery, conn);
                cmd.Parameters.AddWithValue("@orderid", orderId);
                cmd.Parameters.AddWithValue("@itemid", itemId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@price", price);
                conn.Open();
                //cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Item added to order.");
        }
        Console.WriteLine("Order successfully placed!");
    }

}