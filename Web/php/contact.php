<form name="contactform" method="post" action="send_form_email.php">
	<table width="450px">
		<tr>
			<td valign="top">
			<label for="name">Name *</label>
			</td>
			<td valign="top">
			<input  type="text" name="name" maxlength="50" size="30">
			</td>
		</tr>
		<tr>
			<td valign="top">
			<label for="email">Email Address *</label>
			</td>
			<td valign="top">
			<input  type="text" name="email" maxlength="80" size="30">
			</td>
		</tr>
		<tr>
			<td valign="top">
			<label for="comments">Comments *</label>
			</td>
			<td valign="top">
			<textarea  name="comments" maxlength="1000" cols="25" rows="6"></textarea>
			</td>
		</tr>
		<tr>
			<td colspan="2" style="text-align:center">
			<input type="submit" value="Submit">
			</td>
		</tr>
	</table>
</form>
<?php
	if(isset($_GET['status']))
	{
		if($_GET['status']=='success')
		{
			echo "Thank you for contacting us. We will be in touch with you very soon.";
		}
		else if ($_GET['status']=='error')
		{
			echo "We are sorry, but there appears to be a problem with the form you submitted.";
			if(isset($_GET['code']))
			{
				if($_GET['code']=='1')
				{
					echo "</br>Name invalid";
				}
				if($_GET['code']=='2')
				{
					echo "</br>Email invalid";
				}
				if($_GET['code']=='3')
				{
					echo "</br>Coments invalid";
				}
				if($_GET['code']=='4')
				{
					echo "</br>All fields required";
				}
			}
		}
	}
?>