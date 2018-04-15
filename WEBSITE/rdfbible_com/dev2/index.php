<?php
include_once("../arc2/ARC2.php");

$defaultQuery = 
'
PREFIX person: <http://www.rdfbible.com/dev2/ns/person.owl> .
SELECT ?personName ?childName ?gender ?hadVision ?hadDream
WHERE
{
   ?person person:name ?personName.
   ?person person:hasChild ?child.
   ?child person:name ?childName.
OPTIONAL
{
   ?person person:gender ?gender.
   ?person person:hadVision ?hadVision.
   ?person person:hadDream ?hadDream.   
}
}
';

?>
<form method="POST">
	Data Query: <textarea name="DATA_QUERY" rows="10" cols="50"><? echo $_POST['DATA_QUERY'] ? $_POST['DATA_QUERY'] : $defaultQuery ?></textarea><br>
        <input type="submit">
</form>
<?

$config = array(
  /* db */
  'db_host' => '????????????????????????',
  'db_name' => '???????????', /* bibleowlrdf_dev2 */
  'db_user' => '????????????',
  'db_pwd' => '????????????',
  /* store */
  'store_name' => 'arc_tests',
  /* network */
  //'proxy_host' => '192.168.1.1',
  //'proxy_port' => 8080,
  /* parsers */
  'bnode_prefix' => 'bn',
  /* sem html extraction */
  'sem_html_formats' => 'rdfa microformats',
);
$store = ARC2::getStore($config);
$q = "";
if ($_POST['DATA_QUERY'])
{
	$q = $_POST['DATA_QUERY'];
}

if ($q != "")
{
$r = '';
$rows = $store->query($q, 'rows');

	if ($errs = $store->getErrors()) {
		/* $errs contains errors from the store and any called 
		sub-component such as the query processor, parsers, or
		the web reader */
	
		$e = '';
		foreach ($errs as $err) {
			$e .= '<li>' . $err . '</li>';
		}
		echo $e ? '<ul>' . $e . '</ul>' : 'no errors found';
	}

if ($rows) {
  foreach ($rows as $row) {
    $r .= '<li>Person Name: ' . $row['personName'] . ', Gender: ' . $row['gender'] . ', Child Name: ' . $row['childName'] . ', HadVision: ' . $row['hadVision'] . ', HadDream: ' . $row['hadDream'] . '</li>';
  }
}

echo $r ? '<ul>' . $r . '</ul>' : 'no records found';
}
else
{
echo 'please enter a query';
}

?>
