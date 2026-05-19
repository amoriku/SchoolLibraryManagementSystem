import { useEffect, useState } from "react";
import type { BaseInputProps, SearchInputProps } from "../utils/props/CustomInputProps";

export const BaseInput = (
    {
        type = "text",
        placeholder,
        required,
        value,
        name,
        error,
        onChange,
        ...rest
    }: BaseInputProps) => {

    return (
        <input
            type={type}
            className={`text-center w-full px-4 py-2 bg-slate-500 rounded-lg outline-none caret-white placeholder:text-gray-300 border focus:placeholder:text-transparent focus:border-slate-800 focus:bg-slate-600 transition-all| 
                ${ error ? 'border-red-500' : ''}`} 
            placeholder={placeholder}
            required={required}
            name={name}
            onChange={onChange}
            {...rest}
        />
    )
}

export const SearchInput = ({ placeholder, onSearch }: SearchInputProps) => {
    const [isFocused, setIsFocused] = useState(false);
    const [searchTerm, setSearchTerm] = useState("")

    useEffect(() => {
        const delayDebounceFn = setTimeout(() => {
            if (searchTerm.trim()) {
                onSearch(searchTerm)
            }
        }, 500);

        return () => clearTimeout(delayDebounceFn)
    }), [searchTerm, onSearch]

    return (
        <div >
            <input
                className="w-full px-4 py-2 bg-slate-100 rounded-lg outline-none caret-indigo-600 placeholder:text-slate-400 focus:bg-white border focus:border-indigo-500 transition-all"
                type="text"
                placeholder={placeholder}
                onChange={(e) => setSearchTerm(e.target.value)}
                onFocus={() => setIsFocused(true)}
                onBlur={() => setIsFocused(false)}

                value={searchTerm}
            />

            {searchTerm && (
                <div className="flex justify-center mt-2">
                    <button
                        onClick={() => setSearchTerm("")}
                        className="absolute right-2 top-1 text-slate-400 hover:text-slate-600"
                    >
                        X
                    </button>
                </div>
            )}
        </div>
    )
}