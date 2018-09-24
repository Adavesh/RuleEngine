
This is a .NET Console Based Application. You can add new rules, remove existing rules or test the rules against the signal file. The rules are stored in a local database file (RuleEngine.mdf). 

CONCEPT:
-------
SignalBroadcaster broadcasts the signals which will be notified to one or more SignalReceivers. As soon as a signal is received by the signal reciever, it is validated against all the rules in the system. If a signal fails against ANY rule, its source ID will be is displayed. Following are the classes involved

Signal            : Model for representing a signal
Rule              : Model for representing a rule
RuleManager       : Manages the rules
SignalBroadcaster : A class that acts as an origin of the signals
SignalReceiver	  : A class that is registered with the SignalBroadcaster and runs rules on Signal as soon as it is received.

Assumptions:
------------
1. The rule conditions should be one of =, !=, <, >, <=, >=
2. For rules to validate strings, only = and != are considered.
3. For DatTime rule, along with specifying date, user can specify TODAY to denote todays date. This makes the rule hold good on any day which might check future signal dates.

Performance:
-----------
Runtime performance of running a rule is O(N) where N is number of Rules in the system.

Improvements:
------------
The solution can be multithreaded to speed up the running of rules.


USAGE:
------

Add New Rule:
-------------
Use "RuleEngine ADD <New_Rule>" command to add new rule. The rule should be in following format (spacing added for readablility)
{signal:<source id>, condition:<predefined_condition>, target_value:<Signal value to validate>,value_type:<type of the value>}

Remove Rule:
------------
Use RuleEngine Remove <Rule> command to remove an existing rule. Format of <Rule> should be as mentioned above. You can also use * to remove all rules from the program.

Execute:
--------
Use command RuleEngine <RulesFile> to run all the rules agaist the signal objects in the input file. 


