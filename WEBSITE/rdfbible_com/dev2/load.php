<?php
include_once("../arc2/ARC2.php");

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

if (!$store->isSetUp()) {
  $store->setUp();
}

$store->query('LOAD <http://www.rdfbible.com/dev3/bible/bible.rdf>');
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
echo 'data has been loaded.';

?>
