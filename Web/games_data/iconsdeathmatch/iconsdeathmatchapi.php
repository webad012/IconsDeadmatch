<?php
include("MyTXT.php");

$action = $_REQUEST['action'];

if($action == 'ValidateProfile')
{
	$username = $_REQUEST['Username'];
	$password = $_REQUEST['Password'];
	
	if(!isset($username) || !isset($password))
	{
		echo 'bad action';
	}
	else
	{	
		$mytxt = new MyTXT("profiles.txt");
	
		$exists = false;
		foreach ($mytxt->rows as $row) 
		{
			if($row['username'] == $username
				&& $row['password'] == $password)
			{
				$exists = true;
				break;
			}
		}
		
		if($exists)
		{
			echo "OK";
		}
		else
		{
			echo "ERROR";
		}
		
		$mytxt->close();
	}
}
else if($action == 'CreateProfile')
{
	$username = $_REQUEST['Username'];
	$password = $_REQUEST['Password'];
	$email = $_REQUEST['Email'];
	
	if(!isset($username) || !isset($password) || !isset($email))
	{
		echo 'bad action';
	}
	else
	{	
		$mytxt = new MyTXT("profiles.txt");
	
		$exists = false;
		foreach ($mytxt->rows as $row) 
		{
			if($row['username'] == $username)
			{
				$exists = true;
				break;
			}
		}
		
		if(!$exists)
		{
			$mytxt->add_row(array($username, $password, $email));
			$mytxt->save("profiles.txt");
			echo "OK";
		}
		else
		{
			echo "Account with given username exists";
		}
		
		$mytxt->close();
	}
}
else if($action == 'DeleteProfile')
{
	$username = $_REQUEST['Username'];
	
	if(!isset($username))
	{
		echo 'bad action';
	}
	else
	{	
		$mytxt = new MyTXT("profiles.txt");
	
		$row_index = -1;
		$cnt = 0;
		foreach ($mytxt->rows as $row) 
		{
			if($row['username'] == $username)
			{
				$row_index = $cnt;
				break;
			}
			
			$cnt++;
		}
		
		if($row_index != -1)
		{
			$mytxt->remove_row($row_index);
			$mytxt->save("profiles.txt");
		}
		
		$mytxt->close();
	}
}
else if($action == 'RecoverPassword')
{
	$username = $_REQUEST['Username'];
	
	if(!isset($username))
	{
		echo 'bad action';
	}
	else
	{
		$mytxt = new MyTXT("profiles.txt");
	
		$exists = false;
		$email = "";
		$message = "Password: ";
		foreach ($mytxt->rows as $row) 
		{
			if($row['username'] == $username)
			{
				$exists = true;
				$email = $row['email'];
				$message .= $row['password'];
				break;
			}
		}
		
		if($exists)
		{
			mail($email, 'Icons Deathmatch password recovery', $message, 'From: Icons Deathmatch mail system <webad012@gmail.com>');
			echo "OK";
		}
		else
		{
			echo "user not found";
		}
		
		$mytxt->close();
	}
}
else if($action == 'GetVersuses')
{
	$mytxt = new MyTXT("versuses.txt");
	
	$result = "";
	foreach ($mytxt->rows as $row) 
	{
		if($result != "")
		{
			$result .= "|";
		}
	
		$result .= $row["team1"]."|".$row["team2"];
	}
	echo $result;
	
	$mytxt->close();
}
else if($action == 'SendScore')
{
	$username = $_REQUEST['Username'];
	$teamname = $_REQUEST['Team'];
	$score = $_REQUEST['Score'];
	
	if(!isset($username) || !isset($teamname) || !isset($score))
	{
		echo 'bad action';
	}
	else
	{
		$mytxt = new MyTXT("scores.txt");
		
		$newrow = array($username, $teamname, $score);
		$mytxt->add_row($newrow);
		$mytxt->save("scores.txt");
		$mytxt->close();
		
		echo "OK";
	}
}
else if($action == 'GetTeamScores')
{
	$team = $_REQUEST['Team'];
	
	if(!isset($team))
	{
		echo 'bad action';
	}
	else
	{
		$mytxt = new MyTXT("scores.txt");
		
		$array_result = array();
		foreach ($mytxt->rows as $row) 
		{
			if($row["teamname"] === $team)
			{
				if(array_key_exists($row["username"], $array_result))
				{
					$array_result[$row["username"]] += $row["score"];
				}
				else
				{
					$array_result[$row["username"]] = $row["score"];
				}
			}
		}
		
		$mytxt->close();
		
		arsort($array_result);
		$result = "";
		foreach($array_result as $key => $value)
		{
			if($result != "")
			{
				$result .= "|";
			}
		
			$result .= $key."|".$value;
		}
		echo $result;
	}
}
else if($action == 'GetCurrentVersion')
{
	$mytxt = new MyTXT("currentversion.txt");
	
	$result = $mytxt->rows[0]["current_version"];
	
	$mytxt->close();
	
	echo $result;
}
else if($action == 'SuggestVS')
{
	$team1 = $_REQUEST['Team1'];
	$team2 = $_REQUEST['Team2'];
	$comment = $_REQUEST['Comment'];
	
	if(!isset($team1) || !isset($team2) || !isset($comment))
	{
		echo 'bad action';
	}
	else
	{	
		$mytxt = new MyTXT("suggestedvss.txt");
	
		$exists = false;
		foreach ($mytxt->rows as $row) 
		{
			if(($row['team1name'] == $team1 && $row['team2name'] == $team2)
				|| ($row['team1name'] == $team2 && $row['team2name'] == $team1))
			{
				$exists = true;
				break;
			}
		}
		
		if(!$exists)
		{
			$mytxt->add_row(array($team1, $team2, $comment));
			$mytxt->save("suggestedvss.txt");
		}
		
		$mytxt->close();
		
		echo "OK";
	}
}
else
{
	echo 'bad action';
}
?>