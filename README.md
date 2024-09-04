# 👋 Click And Collect Application
## Website
## Product Description

Click and Collect is a modern solution that helps users order food online and conveniently go to the store to pick it up. With this app, users can browse the menu of their favorite restaurants or eateries, choose the dishes they want, then order online and pay through the app.


## Why Choose Our Website?

The Click and Collect app offers a quick and convenient shopping experience, while optimizing the food ordering and collection process for both users and restaurants.

## 🚍 Key Features
- **User Benefits**:
    - Time-saving: Users can order in advance and pick up their food when it is ready, no need to wait in line.
    - Convenient: Users can view the menu, order, and pay with just a few taps on their phone.

- **Admin Panel**:
  - Product Management: Admin has full control over products including adding, editing, deleting products.
  - Coupon Management: Admin can activate, create a new coupon in a certain event.
  - Process Management: Admin can view order status, can cancel order or mark as completed.

- **Security**:
  - Secure authentication and user data protection to ensure privacy and safety.
  - Regular security updates and maintenance to protect against vulnerabilities.

These features collectively make the Click and Collect platform a comprehensive and user-friendly solution for both users and restaurants.
## ✨ Technologies Used

### Frontend

- **HTML**: Used to build the structure of the web pages.
- **CSS**: CSS is used for styling and layout of the web pages.
- **Bootstrap**: A popular CSS framework that helps create responsive and easy-to-use designs.

### Backend

- **ASP.NET Core Web API (C#)**: A powerful framework from Microsoft for building web applications API pattern, ensuring clear organization and maintainability of the code.
- **Entity Framework**: Microsoft’s ORM (Object-Relational Mapping) that facilitates easy and efficient database interactions, minimizing manual SQL code and ensuring data consistency.

### Database

- **SQL Server**: A robust and scalable relational database management system from Microsoft, used to manage and store the application's data securely.
## Project Timeline

This project was completed from June 11, 2024, to August 11, 2024.

## 🍼 Running the Application

### Prerequisites

- .NET Core SDK
- Entity Framework Core
- A database server (SQL Server)

### Steps

1. Clone the repository:  
   ```bash
   git clone https://github.com/LapTrinhKhongCode/Click_And_Collect.git
   ```

2. Set up the database:  
Update the connection string in each sub project's <sub>appsettings.json</sub> to point to your database server.
		   ```
     		   "ConnectionStrings": {
		   "MyContext": "YOUR CONNECTION STRING DATABASE"
		   }
     		   ```
> [!NOTE]
> For security reasons, if you need the database file to import and use, please contact me via email or Facebook (contact details provided below). I cannot send it directly here.

3. Open Visual Studio (Or Visual Studio Code but I recommend Visual Studio )  

- **Run the application: Choose to run multiple projects at once (Select all projects)**  
4. Access the application:  
- Open a web browser and navigate to http://localhost:xxxx to access the Click And Collect website.

>⚠️ **Note:** To be able to assign permissions to new accounts, go to the Register.cshtml file and remove the 'hidden' attribute of the div tag and the 'disable' attribute of the option tag in 'Role Session'.

## Video Introduction and Feature Demonstration

For a detailed introduction and demonstration of the application's features, please watch the following video:

[VideoDemo](https://drive.google.com/file/d/1lS0UMRkifBBTfJ4xnY-Rewm8HOLbRiXN/view?usp=sharing)
>⚠️ **Note:** The video demo does not cover all the features of the website and might sometimes be difficult to visualize. 

> **Additional Note:** When watching the video, please set the resolution to high (720pHD, 1080pHD) for clear viewing instead of the default setting.

## Contact

For further information or any queries, please contact us at:  

+ **Email:** dovanducanh06@gmail.com  

+ **Facebook:** facebook.com/kai.mataashita


## 🚶 Contributing

We welcome contributions to enhance our Click And Collect web application! Whether you have a suggestion for a new feature, an improvement to existing functionality, or a bug fix, your input is valuable. Here’s how you can contribute:

### How to Contribute

1. **Fork the Repository:**  
   - Navigate to our GitHub repository.  
   - Click the "Fork" button at the top right of the page to create a copy of the repository under your GitHub account.

2. **Create a Feature Branch:**  
   - Clone your forked repository to your local machine.  
   - Create a new branch for your feature or improvement:  
     ```bash
     git checkout -b feature/YourBranch
     ```

3. **Implement Your Changes:**  
   - Make the necessary changes in your branch.  
   - Ensure your code follows the project's coding standards and passes all tests.

4. **Commit Your Changes:**  
   - Write clear and concise commit messages:  
     ```bash
     git commit -m 'Add some NewFeature'
     ```

5. **Push to the Branch:**  
   - Push your changes to your forked repository:  
     ```bash
     git push origin feature/YourBranch
     ```

6. **Create a Pull Request:**  
   - Navigate to the original repository and click the "New Pull Request" button.  
   - Provide a descriptive title and detailed description of your changes.  
   - Submit the pull request for review.

### Reporting Issues

If you encounter any issues or have ideas for improvements, feel free to open an issue. Please include the "enhancement" tag for feature suggestions. This helps us categorize and prioritize your input effectively 😄

### Tips for a Successful Contribution

- **Follow the Code of Conduct:** Respect all contributors and maintain a positive, collaborative environment.  
- **Stay Consistent:** Adhere to the coding style and guidelines outlined in the project.  
- **Test Thoroughly:** Ensure your changes are well-tested and do not break existing functionality.  
- **Be Descriptive:** Provide clear and detailed descriptions in your pull requests and issues to facilitate understanding and review.

### Acknowledgement

Don’t forget to star the project if you find it useful! This helps others discover the project and shows your support. Thank you for contributing to the Click And Collect web application. Your efforts help improve the platform for all users!

		
 		

