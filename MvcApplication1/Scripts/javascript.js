

function checkForEmptyFields()
{ 


	var userName=document.getElementById("NameField").value;
	var password=document.getElementById("PasswordField");
	var text = document.getElementById("TextField").value;
	var checkbox = document.getElementById("check");
	

	if (userName == "" || password == "" || text == "" || checkbox.checked == false )
	{
		alert("Please Enter All Fields");
	}
}