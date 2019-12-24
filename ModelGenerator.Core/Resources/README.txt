fmt file is a FORMAT file for model generator
you can write any language into the .fmt file with
embeded directives

directives are split into 2 categories : Header and Decorator

Possible Header directives:
    - [REQUIRED] @typdef:filepath
        |describe the type-difinition for data mapping from DBMS to specified-language.
    - [REQUIRED] @ext:.yourextension
        |specified the file extension after generated.
    - @nullable:format
        |specified that the language is support nullable data type
        |format can include @type.
	- @partial
		|specified that the langauge is support multiple-partial files.
		|partial file do not contains any property (@properties had no effects there).

Possible Decorator directives:
    - @namespace:format
        |specified that this directive will render if the namespace is supplied.
        |format can include @namespace_name.
    - @#namespace
        |specified that this directive will render the end of namespace so no parameter required.
    - @class_name
        |specified the class name, this is not a required directive but it should be mandatory.
    - @properties:format
        |specified the property-format, this directive will render multiple times according to columns in database.
        |this directive can be uses multiple times if the property requires more than 1 line.
        |format can include @property_type, @property_name.


there are a special directives extended the usage for Unit of Work generator listed below.
Possible Decorator directives:
    - @database_type
    - @database_parameter_type