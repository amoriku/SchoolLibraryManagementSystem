import { useEffect, useState } from "react";
import type { BaseInputProps, SearchInputProps } from "../utils/props/CustomInputProps";
import { BiSearch } from "react-icons/bi";

export const BaseInput = (
    {
        type = "text",
        placeholder,
        required,
        value,
        name,
        error,
        onChange,
        width,
        ...rest
    }: BaseInputProps) => {

    return (
        <>
            <input
                type={type}
                className={error ? 'incorrect-input' : 'correct-input'}
                placeholder={placeholder}
                required={required}
                name={name}
                onChange={onChange}
                width={width}
                {...rest}
            />
        </>
    )
}

export const SearchInput = ({ placeholder, onSearch }: SearchInputProps) => {
    const [isFocused, setIsFocused] = useState(false);
    const [searchTerm, setSearchTerm] = useState("")

    // useEffect(() => {
    //     const delayDebounceFn = setTimeout(() => {
    //         if (searchTerm.trim()) {
    //             onSearch(searchTerm)
    //         }
    //     }, 500);

    //     return () => clearTimeout(delayDebounceFn)
    // }), [searchTerm, onSearch]

    return (
        <div className="">
            <div className="flex p-2 rounded-xl border items-center border-slate-200 transition-all">
                <input
                    className="caret-emerald-600 outline-none"
                    type="text"
                    placeholder={placeholder}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    onFocus={() => setIsFocused(true)}
                    onBlur={() => setIsFocused(false)}

                    value={searchTerm}
                />
                {searchTerm && (
                    <button onClick={() => onSearch}>
                        <BiSearch className="text-slate-400"></BiSearch>
                    </button>
                )}
            </div>

            {/* {searchTerm && (
                <div className="flex justify-center mt-2">
                    <button
                        onClick={() => setSearchTerm("")}
                        className="absolute right-2 top-1 text-slate-400 hover:text-slate-600"
                    >
                        X
                    </button>
                </div>
            )} */}
        </div>
    )
}