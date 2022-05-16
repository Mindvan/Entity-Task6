using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}